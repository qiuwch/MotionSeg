#ifdef _WIN
#include "StdAfx.h"
#endif
#include "FrameProcessor.h"
#include "cv.h"
#include <iostream>

using namespace std;

void FrameProcessor::Start()
{
  ProcessLoop();
}


void FrameProcessor::ProcessLoop()
{
  capture.open(0);
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
      waitKey(10);
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
	      fire_map.at<unsigned char>(y, x) = 1;
	    }	
	  else
	    {
	      fire_map.at<unsigned char>(y, x) = 0;
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

  imshow("previous_frame", previous_frame);
  waitKey(10);

  Mat offset_frame = Mat(height, width, CV_8UC3);
  Mat diff_mat;
  // absdiff(previous_frame, frame, diff_mat);
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
		
      Vec2i velocity(1, 0);
      DiffFilter(velocity, frame, previous_frame);

    }
  frame.copyTo(previous_frame); 
}

int main()
{
  FrameProcessor processor;
  processor.Start();
  return 0;
}
