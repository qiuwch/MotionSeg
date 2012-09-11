#include "VideoCaptureWrapper.h"

using namespace HbTech;

int main()
{
	VideoCaptureWrapper^ wrapper = gcnew VideoCaptureWrapper();
	if (wrapper->Init())
	{
		wrapper->CaptureLoop();
	}
}