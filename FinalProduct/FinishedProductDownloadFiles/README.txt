Included here is the C# frontend ONLY.
In order to run this project, you will need to compile the python backend files as they are too big to host here.
Step 1: Download the CNN_Predict_Finale.py and the armas_gigas.h5 files from the FinalProduct/PythonBackendSourceCodeAndModel branch.
Step 2: Install the 3.11 Python programming language to your machine. If you are on windows, you can do this through the windows store.
Step 3: Install the listed packages using pip in the commandline: 
	Package: tensorflow		Version: 2.12.0
	Package: keras			Version: 2.12.0 (should be installed with tensorflow)
	Package: opencv-contrib-python  Version: 4.9.0.80
	Package: pandas			Version: 2.2.0
	Package: numpy			Version: 1.23.5
	Package: Pillow			Version: 9.5.0
	Package: silence-tensorflow	Version: 1.2.1
Step 4: Install PyInstaller
Step 5: Run PyInstaller on the python file you downloaded
Step 6: Navigate to the dist/CNN_Predict_Finale file that is with the python file
Step 7: Move the _internal and the CNN_Predict_Finale.exe files to the C# folder
Step 8: Run the C# exe named AppDemo