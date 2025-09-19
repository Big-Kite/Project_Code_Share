#include <xlnt/xlnt.hpp>
#include <iostream>
#include <fstream>
#include <filesystem>
#include <Windows.h>

namespace fs = std::filesystem;

/// <summary>
/// 해당 프로젝트는 엑셀 -> CSV로 컨버팅해주는 코드
/// 코드를 이식하면 std::filesystem 에러는 솔루션 속성 C++/언어/언어표준을 17이상 버전으로 세팅 20 권장
/// #include <xlnt/xlnt.hpp> 전처리를 위한 세팅은 아래와 같다.
/// 1. git clone https://github.com/microsoft/vcpkg.git 으로 패키지 클론
/// 2. 클론 리포지토리 안에서 .\bootstrap-vcpkg.bat 와 .\vcpkg install xlnt 실행
/// 3. 클론 리포지토리 안에서 .\vcpkg integrate install 실행
/// 로컬 PC 전역 비주얼스튜디오 프로젝트에 통합 설정되어 통합 적용된다.
/// </summary>

int main()
{
    // 1. 실행 파일 경로 기준 폴더 참조
    char buffer[MAX_PATH];
    GetModuleFileNameA(NULL, buffer, MAX_PATH);
    fs::path exeDir = fs::path(buffer).parent_path();

    fs::path importDir = exeDir / "ImportPath";
    fs::path exportDir = exeDir / "ExportPath";

    // ImportPath 확인
    if (!fs::exists(importDir)) {
        std::cerr << "[Error] ImportPath Not Exist!.\n";
        return 1;
    }

    // ExportPath 폴더가 없으면 생성, 있으면 내부 CSV 파일 삭제
    if (!fs::exists(exportDir)) {
        fs::create_directory(exportDir);
    }
    else {
        int deletedCount = 0;
        for (const auto& entry : fs::directory_iterator(exportDir)) {
            if (entry.is_regular_file() && entry.path().extension() == ".csv") {
                fs::remove(entry);  // 기존 CSV 삭제
                ++deletedCount;
            }
        }
        std::cout << "ExportPath: " << deletedCount << " CSV file deleted.\n";
    }

    // ImportPath 내부 .xlsx 변환
    for (const auto& entry : fs::directory_iterator(importDir)) {
        if (entry.is_regular_file() && entry.path().extension() == ".xlsx") {
            fs::path excelPath = entry.path();
            std::string baseName = excelPath.stem().string(); // ex: Test1
            fs::path csvPath = exportDir / ("export_" + baseName + ".csv");

            std::cout << "Progress... " << excelPath.filename() << " > " << csvPath.filename() << "\n";

            try {
                xlnt::workbook wb;
                wb.load(excelPath.string());
                auto ws = wb.active_sheet();

                std::ofstream csv(csvPath);
                for (auto row : ws.rows(false)) {
                    bool first = true;
                    for (auto cell : row) {
                        if (!first) csv << "|"; // 구분자 변경: 쉼표 → 파이프
                        csv << cell.to_string();
                        first = false;
                    }
                    csv << "\n";
                }

            }
            catch (const std::exception& e) {
                std::cerr << "[Error] (" << excelPath.filename() << "): " << e.what() << "\n";
            }
        }
    }
    std::cout << "All Convert Complete!\n";
    std::cin.get();
    return 0;
}
