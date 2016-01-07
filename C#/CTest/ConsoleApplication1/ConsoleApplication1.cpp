// ConsoleApplication1.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "ConsoleApplication1.h"


// 这是导出变量的一个示例
CONSOLEAPPLICATION1_API int nConsoleApplication1=0;

// 这是导出函数的一个示例。
CONSOLEAPPLICATION1_API int fnConsoleApplication1(void)
{
	return 42;
}

// 这是已导出类的构造函数。
// 有关类定义的信息，请参阅 ConsoleApplication1.h
CConsoleApplication1::CConsoleApplication1()
{
	return;
}
