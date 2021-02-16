#include <algorithm>

#include "Headers/WordsCounter.h"

namespace CFTTestApp
{
    WordsCounter::WordsCounter()
    {
    }

    void WordsCounter::CountWords(const std::vector<std::string>& str)
    {
        for (auto it = str.begin(); it != str.end(); ++it)
        {
            if (WordsCounts.find(*it) != WordsCounts.end())
            {
                WordsCounts[*it]++;
            }
            else
            {
                WordsCounts[*it] = 1;
            }
        }
    }

    std::vector<std::string> WordsCounter::GetTop(int sizeOfTop)
    {
        std::vector<std::string> top;

        auto comp = [&top](auto& largest, auto& second)
        {
            return largest.second < second.second;
        };

        for (int idx = 0; idx < sizeOfTop; ++idx)
        {
            auto it = std::max_element(WordsCounts.begin(), WordsCounts.end(), comp);
            if (it == WordsCounts.end())
                break;
            auto str = it->first;
            WordsCounts.erase(it);
            top.push_back(str);
        }

        return top;
    }
}