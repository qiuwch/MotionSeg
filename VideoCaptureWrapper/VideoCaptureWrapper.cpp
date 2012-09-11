#include "VideoCaptureWrapper.h"

using namespace HbTech;

VideoCaptureWrapper::VideoCaptureWrapper(void)
{
	capture = new VideoCapture();
}

bool VideoCaptureWrapper::Init(System::String^ filename)
{
	IntPtr strptr = Marshal::StringToHGlobalAnsi(filename);
	char *filename_ptr = reinterpret_cast<char*>(strptr.ToPointer());
	bool flag = capture->open(filename_ptr);
	Marshal::FreeHGlobal(strptr);
	return flag;
}

bool VideoCaptureWrapper::Init()
{
	capture->open("mv2_001.avi");
	return (capture->isOpened());
}

void VideoCaptureWrapper::CaptureLoop()
{
	while(1)
	{
		CaptureFrame();
		cvWaitKey(1);
	}
}

Bitmap^ ConvertFrame(const Mat frame)
{
	int width = frame.cols, height = frame.rows;
	Bitmap^ bitmap_frame = gcnew Bitmap(width, height, PixelFormat::Format24bppRgb);

	System::Drawing::Rectangle rect(0, 0, width, height); 
	BitmapData^ bitmap_data = bitmap_frame->LockBits(rect, ImageLockMode::ReadWrite, PixelFormat::Format24bppRgb);
	 
	IntPtr ptr = bitmap_data->Scan0;
	byte *bitptr = reinterpret_cast<byte*>(ptr.ToPointer());

	int count = frame.dataend - frame.datastart;
	/*
	byte[] copy_image = new byte[count];
	Marshal::Copy(frame.data, copy_image, 0, count);
	*/
	int index = 0;
	for (int i = 0; i < count; i++)
	{
		*bitptr++ = frame.data[index++];
	}

	bitmap_frame->UnlockBits(bitmap_data);	
	return bitmap_frame;
}

Bitmap^ VideoCaptureWrapper::CaptureFrame()
{
	if (!capture->isOpened()) return nullptr;
	capture->grab();
	Mat frame;
	capture->retrieve(frame);
	if (frame.empty()) return nullptr;
	// if (!frame.empty()) imshow("frame", frame);

	Bitmap^ bitmap_frame = ConvertFrame(frame);
	return bitmap_frame;
}

void VideoCaptureWrapper::SetCurrentRatio(double ratio)
{
	capture->set(CV_CAP_PROP_POS_AVI_RATIO, ratio);
}

void VideoCaptureWrapper::SetCurrentFrames(double frame)
{
	capture->set(CV_CAP_PROP_POS_FRAMES, frame);
}

double VideoCaptureWrapper::GetCurrentRatio()
{
	return capture->get(CV_CAP_PROP_POS_AVI_RATIO);
}

int VideoCaptureWrapper::GetCurrentFrames()
{
	return capture->get(CV_CAP_PROP_POS_FRAMES);
}

double VideoCaptureWrapper::GetCurrentMSEC()
{
	return capture->get(CV_CAP_PROP_POS_MSEC);
}

int VideoCaptureWrapper::GetTotalFrames()
{
	return capture->get(CV_CAP_PROP_FRAME_COUNT);
}

int VideoCaptureWrapper::GetFrameRate()
{
	return (int)capture->get(CV_CAP_PROP_FPS);
}

int VideoCaptureWrapper::GetHeight()
{
	return (int)capture->get(CV_CAP_PROP_FRAME_HEIGHT);
}

int VideoCaptureWrapper::GetWidth()
{
	return (int)capture->get(CV_CAP_PROP_FRAME_WIDTH);
}