// ���� ifdef ���Ǵ���ʹ�� DLL �������򵥵�
// ��ı�׼�������� DLL �е������ļ��������������϶���� CONSOLEAPPLICATION1_EXPORTS
// ���ű���ġ���ʹ�ô� DLL ��
// �κ�������Ŀ�ϲ�Ӧ����˷��š�������Դ�ļ��а������ļ����κ�������Ŀ���Ὣ
// CONSOLEAPPLICATION1_API ������Ϊ�Ǵ� DLL ����ģ����� DLL ���ô˺궨���
// ������Ϊ�Ǳ������ġ�
#ifdef CONSOLEAPPLICATION1_EXPORTS
#define CONSOLEAPPLICATION1_API __declspec(dllexport)
#else
#define CONSOLEAPPLICATION1_API __declspec(dllimport)
#endif

// �����Ǵ� ConsoleApplication1.dll ������
class CONSOLEAPPLICATION1_API CConsoleApplication1 {
public:
	CConsoleApplication1(void);
	// TODO:  �ڴ�������ķ�����
};

extern CONSOLEAPPLICATION1_API int nConsoleApplication1;

CONSOLEAPPLICATION1_API int fnConsoleApplication1(void);
