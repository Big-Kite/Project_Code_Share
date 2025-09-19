#include <xlnt/xlnt.hpp>
#include <iostream>
#include <fstream>
#include <filesystem>
#include <Windows.h>

namespace fs = std::filesystem;

/// <summary>
/// �ش� ������Ʈ�� ���� -> CSV�� ���������ִ� �ڵ�
/// �ڵ带 �̽��ϸ� std::filesystem ������ �ַ�� �Ӽ� C++/���/���ǥ���� 17�̻� �������� ���� 20 ����
/// #include <xlnt/xlnt.hpp> ��ó���� ���� ������ �Ʒ��� ����.
/// 1. git clone https://github.com/microsoft/vcpkg.git ���� ��Ű�� Ŭ��
/// 2. Ŭ�� �������丮 �ȿ��� .\bootstrap-vcpkg.bat �� .\vcpkg install xlnt ����
/// 3. Ŭ�� �������丮 �ȿ��� .\vcpkg integrate install ����
/// ���� PC ���� ���־�Ʃ��� ������Ʈ�� ���� �����Ǿ� ���� ����ȴ�.
/// </summary>

int main()
{
    // 1. ���� ���� ��� ���� ���� ����
    char buffer[MAX_PATH];
    GetModuleFileNameA(NULL, buffer, MAX_PATH);
    fs::path exeDir = fs::path(buffer).parent_path();

    fs::path importDir = exeDir / "ImportPath";
    fs::path exportDir = exeDir / "ExportPath";

    // ImportPath Ȯ��
    if (!fs::exists(importDir)) {
        std::cerr << "[Error] ImportPath Not Exist!.\n";
        return 1;
    }

    // ExportPath ������ ������ ����, ������ ���� CSV ���� ����
    if (!fs::exists(exportDir)) {
        fs::create_directory(exportDir);
    }
    else {
        int deletedCount = 0;
        for (const auto& entry : fs::directory_iterator(exportDir)) {
            if (entry.is_regular_file() && entry.path().extension() == ".csv") {
                fs::remove(entry);  // ���� CSV ����
                ++deletedCount;
            }
        }
        std::cout << "ExportPath: " << deletedCount << " CSV file deleted.\n";
    }

    // ImportPath ���� .xlsx ��ȯ
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
                        if (!first) csv << "|"; // ������ ����: ��ǥ �� ������
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
