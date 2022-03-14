#include <iostream>

extern "C" {
    void ENTRY_FUNC();
}

int main() {
    ENTRY_FUNC();
    return 0;
}
