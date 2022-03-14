#include <iostream>

extern "C" {
    char* ENTRY_FUNC();
}

int main() {
    char* result = ENTRY_FUNC();
    std::cout << result << std::endl;
    return 0;
}
