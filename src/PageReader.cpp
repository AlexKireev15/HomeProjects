#include <boost/algorithm/string.hpp>

#include "Headers/PageReader.h"

namespace CFTTestApp
{
    PageReader::PageReader(const std::string& filepath, long long pageSize) :
        Filepath(filepath), PageSize(pageSize), ReaderThread(std::thread(&PageReader::ReadNextPage, this)),
        IsReadNotified(false), IsReadedNotified(false), IsTerminate(false)
    {}

    std::shared_ptr<std::vector<std::string>> SplitStringBy(std::string& original, std::vector<char> delimeters)
    {
        auto result = std::make_shared<std::vector<std::string>>();
        auto delim = boost::is_any_of(delimeters);
        boost::trim_if(original, delim);
        boost::split(*result.get(), original, delim, boost::algorithm::token_compress_on);
        return result;
    }

    void PageReader::ReadNextPage()
    {
        this->In = std::ifstream(Filepath, std::ios::binary);
        while (!IsTerminate && In.is_open() && In.good())
        {
            std::unique_lock<std::mutex> lock(ReaderMutex);

            std::string buffer;
            buffer.resize(PageSize);

            std::streamoff curPos = In.tellg();

            In.read(&buffer[0], PageSize);

            if (!In.eof())
            {
                for (int idx = buffer.length() - 1; idx >= 0; --idx)
                {
                    if (buffer[idx] == ' ' || buffer[idx] == '\t' || buffer[idx] == '\n')
                    {
                        buffer.resize((size_t)idx + 1);
                        break;
                    }
                }
                In.seekg(curPos + buffer.length());
            }

            CurrentPage = SplitStringBy(buffer, std::vector<char>({ ' ', '\t', '\n', '\r', '\0', '\v', '\a', '.', ',', '!', '?', ':', '\"', '\'', '—' }));

            IsReadedNotified = true;
            ReaderReadedCV.notify_one();

            while (!IsReadNotified)
                ReaderReadCV.wait(lock);
            IsReadNotified = false;
        }

        CurrentPage = nullptr;
        IsReadedNotified = true;
        ReaderReadedCV.notify_one();
    }

    std::shared_ptr<std::vector<std::string>> PageReader::GetPage()
    {
        std::unique_lock<std::mutex> lock(ReaderMutex);
        while (!IsReadedNotified)
            ReaderReadedCV.wait(lock);
        IsReadedNotified = false;

        auto page = CurrentPage;
        IsReadNotified = true;
        ReaderReadCV.notify_one();
        return page;
    }

    PageReader::~PageReader()
    {
        this->IsTerminate = true;
        IsReadNotified = true;
        ReaderReadCV.notify_one();
        this->ReaderThread.join();
        this->In.close();
    }
}
