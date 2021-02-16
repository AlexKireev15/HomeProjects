#include <fstream>
#include <math.h>
#include <string>
#include "Headers/PageReader.h"
#include "Headers/WordsCounter.h"

using namespace CFTTestApp;

std::string GetOutFileName(const std::string& inFileName)
{
    std::string out;
    auto dot = std::find(inFileName.begin(), inFileName.end(), '.');
    out.append(inFileName.begin(), dot);
    out.append("_out.txt");
    return out;
}

void PrintResult(const std::string& pathName, const std::vector<std::string>& result)
{
    std::ofstream out(GetOutFileName(pathName), std::ios::trunc);
    for (size_t idx = 0; idx < result.size(); ++idx)
    {
        out << result[idx] << std::endl;
    }
    out.close();
}

int main(int argc, char* argv[]) {

    if (argc < 2)
    {
        return 1;
    }

    PageReader pr(argv[1], PageReader::DefaultPageSize / 10);
    WordsCounter wc;

    while (auto page = pr.GetPage())
    {
        wc.CountWords(*page);
    }
    
    PrintResult(argv[1], wc.GetTop(100));
    return 0;
}