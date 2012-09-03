// MotionSeg.cpp : main project file.

#include "stdafx.h"
#include "LiveForm.h"

using namespace MotionSeg;

[STAThreadAttribute]
int main(array<System::String ^> ^args)
{
	// Enabling Windows XP visual effects before any controls are created
	Application::EnableVisualStyles();
	Application::SetCompatibleTextRenderingDefault(false); 

	// Create the main window and run it
	Application::Run(gcnew LiveForm());
	return 0;
}
