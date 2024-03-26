import cv2
from keras import models
import numpy as np
import pandas as pd
import os
import glob
import time
from PIL import Image
from PIL.ExifTags import TAGS
import sys

np.set_printoptions(threshold=np.inf)
np.set_printoptions(suppress=True)

#loading the model and setting the image size
model = models.load_model('bighorn2.h5')
image_size = (225, 300)

#temporary lists to hold data before turning them into dataframes
file_names = []
directory = str(sys.argv[1])
study_area = str(sys.argv[2])
site_name = str(sys.argv[3])
#study_area = "WISOC"
#site_name = "TH616_B"
time_stamps = []
certainty_levels = []
species_identifications = []

identification = ""
date_time = ""

#grabbing all matching image files from the selected directories
os.chdir(directory)
for file in glob.glob("*.jpg"):
    img = cv2.imread(file)
    img = cv2.resize(img, image_size)
    img_array = np.array(img)
    img_array = np.expand_dims(img_array, axis=0)

    #getting the predicted label for each image
    networkOutput = model.predict(img_array)
    print(networkOutput)
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
    temp_certainty = f"Bighorn: {networkOutput[0][0]},  Other Wildlife: {networkOutput[0][2]}, Nothing: {networkOutput[0][1]}"

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
    certainty_levels.append(temp_certainty)
    species_identifications.append(identification)

#putting lists into a tempory dictionary to turn it into a dataframe
temp_dict = {"Image Name": file_names, "Study Area": study_area, "Site Name": site_name, "Time Stamp": time_stamps, "Certainty Levels": certainty_levels, "Identification": species_identifications}

#turning the dataframe into a csv file that will end up in the same directory as the photos
df = pd.DataFrame(temp_dict)
print(df.to_string())
csv_filename = f"{study_area}-{site_name}.csv"
#df.to_csv(csv_filename)
