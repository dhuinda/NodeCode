#include <iostream>

extern "C" {
    bool ENTRY_FUNC();
}

int main() {
    bool result = ENTRY_FUNC();
    std::cout << result << std::endl;
    return 0;
}
