#pragma once
#include <string>
#include <fstream>
#include <vector>
#include <thread>
#include <mutex>
#include <condition_variable>



namespace CFTTestApp
{

#define DEFAULT_PAGE_SIZE

    class PageReader
    {
    public:
        static const long long DefaultPageSize = 536870912;
    private:
        std::string Filepath;
        std::ifstream In;
        long long PageSize;

        std::shared_ptr<std::vector<std::string>> CurrentPage;

        std::thread ReaderThread;
        std::mutex ReaderMutex;
        std::condition_variable ReaderReadCV;
        std::condition_variable ReaderReadedCV;
        bool IsReadNotified;
        bool IsReadedNotified;
        bool IsTerminate;
    public:
        PageReader(const std::string& filepath, long long pageSize = DefaultPageSize);
        std::shared_ptr<std::vector<std::string>> GetPage();
        bool IsEOF();
        ~PageReader();
    private:
        void ReadNextPage();
    };
}