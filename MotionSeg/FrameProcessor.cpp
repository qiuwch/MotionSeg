#include "StdAfx.h"
#include "FrameProcessor.h"
#include "cv.h"
#include <iostream>

using namespace std;

FrameProcessor::FrameProcessor(void)
{

}

void FrameProcessor::Start()
{
	/*
	CvCapture *cap = cvCaptureFromAVI("oc1R2RC.avi");
	cvNamedWindow("test");
	while(1)
	{
		IplImage *f = cvQueryFrame(cap);
		cvShowImage("test", f);
		cvWaitKey(1);
	}
	*/
	// cvNamedWindow("image");
	ProcessLoop();
}


void FrameProcessor::ProcessLoop()
{
	// capture.open(0);
	// capture.open("oc1R2RC.avi");
	// capture.open("mv2_001.avi");
	// capture.open("data.avi");
	capture.open("E:\\Qiuwch\\GitRepo\\matlab\\random_moving\\random_FPS20.avi");
	namedWindow("raw");
	thres = 5;
	createTrackbar("trackbar", "raw", &thres, 20);
	while(1)
	{
		capture.grab();
		capture.retrieve(frame);
		if (frame.empty()) break;
		ProcessFrame();
		imshow("raw", frame);
		waitKey(1000);
	}
}

Mat FireNeuronMap(Mat diff_mat, int thres)
{
	int width = diff_mat.cols, height = diff_mat.rows;
	imshow("raw_scale", diff_mat);

	Mat small, large;
	medianBlur(diff_mat, small, 5);
	medianBlur(diff_mat, large, 5);
	imshow("small_scale", small);
	imshow("large_scale", large);
	waitKey(10);

	Mat fire_map = Mat(Size(width, height), CV_8U);
	for (int x = 0; x < diff_mat.cols; x++)
	{
		for (int y = 0; y < diff_mat.rows; y++)
		{
			Vec3b diff = diff_mat.at<Vec3b>(y, x);
			bool flag = true;
			int sum = 0;
			for (int ind = 0; ind < 3; ind++)
			{
				sum += diff.val[ind];
				/*
				// if (abs(diff.val[ind]) > thres)
				if (diff.val[ind] > thres)
				{
					flag = false;
				}
				*/
			}
			if (sum > thres) flag = false;

			// if < thres, the neuron will be fired
			if (flag) 
			{
				fire_map.at<unsigned char>(y+offy, x+offx) = 1;
			}	
			else
			{
				fire_map.at<unsigned char>(y+offy, x+offx) = 0;
			}
		}
	}

	return fire_map;
}


Mat DiffFilter(Vec2i velocity, Mat& frame, Mat& previous_frame)
{
	int dx = velocity.val[0];
	int dy = velocity.val[1];

	int width = frame.cols, height = frame.rows;
	
	Mat offset_frame = Mat()

	// resize diff_mat to the original size
	Mat diff_mat = Mat()
	absdiff(previous_frame, frame, diff_mat);
	frame.adjustROI(absy, absy, absx, absx); // reset roi of frame
	previous_frame.adjustROI(absy, absy, absx, absx);


	return diff_mat;
}

Mat DiffFireNeuronMap(Vec2i velocity, Mat& frame, Mat& previous_frame, int thres)
{
	Mat diff_mat = DiffFilter(velocity, frame, previous_frame);
	Mat fire_map = FireNeuronMap(diff_mat, thres);
	return fire_map;
}

void FrameProcessor::ProcessFrame()
{
	if(!previous_frame.empty())
	{
		// For different velocity, compute a difference matrix.
		int width = 640, height = 480;
		
		// Consider one dimension only, for better realtime preview performance
		// Mat fire_maps[21];
		int vx1 = -5, vx2 = 5;

		Vec2i zero_velocity(0, 0);
		Mat static_map = DiffFireNeuronMap(zero_velocity, frame, previous_frame, thres); // will not respond to moving object
		// same to bg substraction, high responce to static background
		imshow("static", static_map * 255);

		Vec2i x1_velocity(1, 0);
		Mat x1_map = DiffFireNeuronMap(x1_velocity, frame, previous_frame, thres);
		imshow("x1", x1_map * 255); 
		// High response to x5 velocity 

		/*
		Mat sum_map = Mat::zeros(height, width, CV_8U);
		// Mat sum_map;
		for (int vx = vx1; vx <= vx2; vx++)
		{
			if (vx == 0) continue;
			Vec2i velocity(vx, 0);
			Mat fire_map1 = DiffFireNeuronMap(velocity, frame, previous_frame, thres);
			sum_map = sum_map + (fire_map1 - static_map);
		}
		imshow("sum_map", sum_map * (255 / (vx2 - vx1 + 1))) ;
		*/

		/*
		// Find unique response
		Mat unique_map = Mat(Size(width, height), CV_8U);
		for (int x = 0; x < sum_map.cols; x++)
		{
			for (int y = 0; y < sum_map.rows; y++)
			{
				int rp_count = sum_map.at<unsigned char>(y, x);
				if (rp_count <= 2)
				{
					unique_map.at<unsigned char>(y, x) = 255;
				}
				else
				{
					unique_map.at<unsigned char>(y, x) = 0;
				}
			}
		}
		imshow("unique", unique_map);
		*/


		
	}
	frame.copyTo(previous_frame); 
}