#include "StdAfx.h"
#include "FrameProcessor.h"
#include <iostream>

using namespace std;

FrameProcessor::FrameProcessor(void)
{

}

void FrameProcessor::Start()
{
	capture.open(0);
	// cvNamedWindow("image");
	ProcessLoop();
}

void FrameProcessor::ProcessLoop()
{
	namedWindow("raw");
	thres = 5;
	createTrackbar("trackbar", "raw", &thres, 20);
	while(1)
	{
		capture.retrieve(frame);
		ProcessFrame();
		imshow("raw", frame);
		// imshow("process", result_frame);
		waitKey(10);
	}
}

Mat DiffFireNeuronMap(Vec2i velocity, Mat& frame, Mat& previous_frame, int thres)
{
	int dx = velocity.val[0];
	int dy = velocity.val[1];

	int width = frame.cols, height = frame.rows;
	
	// Show those fired neuron
	// int thres = 5; // in case of capture device noise. 
	// Use gaussian to tolerate the noise

	int offx = dx > 0 ? dx : 0, offy = dy > 0 ? dy : 0;
	int absx = abs(dx), absy = abs(dy);
	if (dx > 0)
	{
		previous_frame.adjustROI(0, 0, 0, -absx);
		frame.adjustROI(0, 0, -absx, 0);
	}
	if (dx < 0)
	{
		previous_frame.adjustROI(0, 0, -absx, 0);
		frame.adjustROI(0, 0, 0, -absx);
	}

	if (dy > 0) // down
	{
		previous_frame.adjustROI(0, -absy, 0, 0);
		frame.adjustROI(-absy, 0, 0, 0);
	}
	if (dy < 0)
	{
		previous_frame.adjustROI(-absy, 0, 0, 0);
		frame.adjustROI(0, -absy, 0, 0);
	}

	// resize diff_mat to the original size
	Mat diff_mat; 
	absdiff(previous_frame, frame, diff_mat);
	frame.adjustROI(absy, absy, absx, absx); // reset roi of frame
	previous_frame.adjustROI(absy, absy, absx, absx);

	Mat fire_map = Mat(Size(width, height), CV_8U);
	for (int x = 0; x < diff_mat.cols; x++)
	{
		for (int y = 0; y < diff_mat.rows; y++)
		{
			Vec3b diff = diff_mat.at<Vec3b>(y, x);
			bool flag = true;
			for (int ind = 0; ind < 3; ind++)
			{
				// if (abs(diff.val[ind]) > thres)
				if (diff.val[ind] > thres)
				{
					flag = false;
				}
			}

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

		Vec2i x5_velocity(5, 0);
		Mat x5_map = DiffFireNeuronMap(x5_velocity, frame, previous_frame, thres);
		imshow("x5", (x5_map - static_map)* 255); 
		// High response to x5 velocity 

		
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

		/*
		Mat diff_mat(Size(width, height), CV_8UC3);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Vec3b vec1 = previous_frame.at<Vec3b>(y,x), vec2 = frame.at<Vec3b>(y,x);
				Vec3b diff_vec = vec1 - vec2;
				diff_mat.at<Vec3b>(y,x) = diff_vec;
			}
		}
		*/
		// Vec3b val = diff_mat.at<Vec3b>(10, 10);

	}
	frame.copyTo(previous_frame); 
}