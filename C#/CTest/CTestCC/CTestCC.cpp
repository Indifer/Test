// CTestCC.cpp: 主项目文件。

#include "stdafx.h"

#include <iostream>
#include <fstream>
#include "mpAccess.h"

#pragma unmanaged
#pragma comment(lib,"PosterDll.lib")

#pragma managed
using namespace System;
using namespace System::IO;

int main(array<System::String ^> ^args)
{
	Console::WriteLine(L"Hello World");
	
	array<unsigned char, 1>^ arr = File::ReadAllBytes("reader.txt");
	
	int len = arr->Length;	
	unsigned char *data = new unsigned char[len];

	for (int i = 0; i < len; i++)
	{
		data[i] = arr[i];
	}

	int heigth = BitConverter::ToInt32(arr, 0);
	int width = BitConverter::ToInt32(arr, 4);

	int p = PosterMatch(data);
	Console::WriteLine(p);

	//delete data;

	return 0;
}
