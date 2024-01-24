import cv2
from keras import models
import os
import tensorflow as tf
import numpy as np
import os, glob
from numpy import linalg as LA

np.set_printoptions(threshold=np.inf)
np.set_printoptions(suppress=True)

# User Input#
mode = input("Do you want to enable debug mode? (yes or no): ")

# Load Model and Checkpoints#
model = models.load_model('sheep.h5')
model.load_weights(tf.train.latest_checkpoint("checkpoints"))

correct = 0
total = 0
os.chdir("testsheep")
#os.chdir("sheep")
for file in glob.glob("*.jpg"):
    img = cv2.imread(file)
    img = cv2.resize(img, (200, 200))
    img_array = np.array(img)
    img_array = np.expand_dims(img_array, axis=0)
    networkOutput = model.predict(img_array)
    # print(networkOutput)
    # print(np.argmax(networkOutput))

    classDog = np.argmax(networkOutput)
    if classDog == 1:
        correct += 1
    total += 1

    if mode == "yes":
        if classDog == 1:
            cv2.imshow("BighornSheep", img)
            cv2.waitKey(0)
        else:
            cv2.imshow("NotBighornSheep", img)
            cv2.waitKey(0)

"""
os.chdir("..")
os.chdir("nothotdog")
for file in glob.glob("*.jpg"):
    img = cv2.imread(file)
    img = cv2.resize(img, (200, 200))
    img_array = np.array(img)
    img_array = np.expand_dims(img_array, axis=0)
    networkOutput = model.predict(img_array)
    # print(np.argmax(networkOutput))

    classDog = np.argmax(networkOutput)
    if classDog == 1:
        correct += 1
    total += 1

    if mode == "yes":
        if classDog == 0:
            cv2.imshow("HotDog", img)
            cv2.waitKey(0)
        else:
            cv2.imshow("NotHotDog", img)
            cv2.waitKey(0)
"""
print((correct / total) * 100, "%")