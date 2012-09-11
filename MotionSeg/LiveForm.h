#pragma once

#include "FrameProcessor.h"
namespace MotionSeg {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Summary for LiveForm
	/// </summary>
	public ref class LiveForm : public System::Windows::Forms::Form
	{
	public:
		LiveForm(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
			FrameProcessor processor;
			processor.Start();
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~LiveForm()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::PictureBox^  pb_frame;
	protected: 

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->pb_frame = (gcnew System::Windows::Forms::PictureBox());
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pb_frame))->BeginInit();
			this->SuspendLayout();
			// 
			// pb_frame
			// 
			this->pb_frame->Location = System::Drawing::Point(1, 1);
			this->pb_frame->Name = L"pb_frame";
			this->pb_frame->Size = System::Drawing::Size(640, 480);
			this->pb_frame->TabIndex = 0;
			this->pb_frame->TabStop = false;
			// 
			// LiveForm
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(696, 483);
			this->Controls->Add(this->pb_frame);
			this->Name = L"LiveForm";
			this->Text = L"Form1";
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pb_frame))->EndInit();
			this->ResumeLayout(false);

		}
#pragma endregion
	};
}

