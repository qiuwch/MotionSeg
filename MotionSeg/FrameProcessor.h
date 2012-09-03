#pragma once
#include "highgui.h"

using namespace cv;

class FrameProcessor
{
private:
	VideoCapture capture;
	Mat frame;
	Mat result_frame;
	Mat previous_frame;
	int thres;
	// VideoCapture capture;
public:
	FrameProcessor(void);

	void ProcessLoop();
	void ProcessFrame();
	void Start();
};

