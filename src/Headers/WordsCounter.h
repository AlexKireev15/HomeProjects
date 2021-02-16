#pragma once
#include <string>
#include <vector>
#include <unordered_map>

namespace CFTTestApp
{
    class WordsCounter
    {
    private:
        std::unordered_map<std::string, int> WordsCounts;
    public:
        WordsCounter();
        void CountWords(const std::vector<std::string>& str);
        std::vector<std::string> GetTop(int sizeOfTop);
        
    };
}