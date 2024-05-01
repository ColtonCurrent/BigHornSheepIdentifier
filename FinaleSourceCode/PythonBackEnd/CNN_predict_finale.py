import cv2
from keras import models
import numpy as np
import pandas as pd
import os
import glob
from PIL import Image
from PIL.ExifTags import TAGS
from silence_tensorflow import silence_tensorflow
silence_tensorflow()
np.set_printoptions(threshold=np.inf)
np.set_printoptions(suppress=True)

#loading the model and setting the image size
model = models.load_model('armas_gigas.h5')
image_size = (225, 300)

#temporary lists to hold data before turning them into dataframes
file_names = []
#uncomment these variables 
#directory = str(sys.argv[1])
#study_area = str(sys.argv[2])
#site_name = str(sys.argv[3])
study_area = "WISOC"
site_name = "TH616_B"
time_stamps = []
level_bighorn = []
level_other = []
level_nothing = []
species_identifications = []

identification = ""
date_time = ""
i = 0
#grabbing all matching image files from the selected directories
#replace this string with the directory variable for command line use. This is just for use in a IDE environment
os.chdir("../predictionAndFileTestingSet")
files = glob.glob("*.jpg")
for file in files:
    i += 1
    print(int((i/len(files))*100))

    try:
        img = cv2.imread(file)
        img = cv2.resize(img, image_size)
        img = cv2.normalize(img, None, 0, 1.0, cv2.NORM_MINMAX, dtype=cv2.CV_32F)
        img_array = np.array(img)
        img_array = np.expand_dims(img_array, axis=0)

        #getting the predicted label for each image
        networkOutput = model.predict(img_array, verbose=0)
        #calculating the accuracy score using the one-hot encoded label for the sheep test photos
        classSheep = np.argmax(networkOutput)

        #getting the timestamp from the photo metadata
        image = Image.open(file)
        exifdata = image.getexif()
        for tagid in exifdata:
            tagname = TAGS.get(tagid, tagid)
            if tagname == "DateTime":
                date_time = exifdata.get(tagid)

        #getting and formatting the certainty levels
        certainty_bighorn = "{:.2f}".format(networkOutput[0][0])
        certainty_other = "{:.2f}".format(networkOutput[0][2])
        certainty_nothing = "{:.2f}".format(networkOutput[0][1])

        #getting the actual identification from argmax function
        if classSheep == 0:
            identification = "Bighorn Sheep"
        elif classSheep == 1:
            identification = "Nothing"
        elif classSheep == 2:
            identification = "Other Wildlife"
        else:
            identification = "Error"

        #appending to our temporary lists
        file_names.append(file)
        time_stamps.append(date_time)
        level_bighorn.append(certainty_bighorn)
        level_other.append(certainty_other)
        level_nothing.append(certainty_nothing)
        species_identifications.append(identification)
    except Exception as e:
        file_names.append(file)
        time_stamps.append("Photo Corruption Likely")
        level_bighorn.append("Error")
        level_other.append("Error")
        level_nothing.append("Error")
        species_identifications.append("Photo Corruption Likely")


#putting lists into a tempory dictionary to turn it into a dataframe
temp_dict = {"Image Name": file_names, "Study Area": study_area, "Site Name": site_name, "Time Stamp": time_stamps, "Bighorn Certainty": level_bighorn, "Other Wildlife Certainty": level_other, "Nothing Certainty": level_nothing, "Identification": species_identifications}
#turning the dataframe into a csv file that will end up in the same directory as the photos
df = pd.DataFrame(temp_dict)
csv_filename = f"{study_area}-{site_name}.csv"
df.to_csv(csv_filename)
