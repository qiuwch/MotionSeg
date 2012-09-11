#pragma once
#include "highgui.h"

using namespace cv;
using namespace System;
using namespace System::Drawing;
using namespace System::Drawing::Imaging;
using namespace System::Runtime::InteropServices;

namespace HbTech
{
	public ref class VideoCaptureWrapper
	{
	private:
		VideoCapture* capture;
	public:
		/*
		int GetCurrentFrames();
		void SetCurrentFrames();
		*/
		int GetTotalFrames();
		int GetCurrentFrames();
		// Had better use this
		double GetCurrentMSEC();
		double GetCurrentRatio();
		int GetFrameRate();
		int GetHeight();
		int GetWidth();

		void SetCurrentRatio(double ratio); // this means height / width ratio?
		void SetCurrentFrames(double frame);

		VideoCaptureWrapper(void);
		bool Init();
		bool Init(System::String^ filename);
		Bitmap^ CaptureFrame();
		void CaptureLoop();
	};
}


