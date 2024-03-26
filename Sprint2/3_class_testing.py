import numpy as np
from keras.preprocessing.image import ImageDataGenerator
from keras import models

#setting image size and loading our saved model
image_size = (300, 225)
model = models.load_model('bighorn3.h5')

#reading in the test data and normalizing it. Set shuffle to False for accurate readings
test_datagen = ImageDataGenerator(rescale=1.0/255)
test_set = test_datagen.flow_from_directory(directory='modifiedTestSet', target_size=image_size, class_mode='categorical', shuffle=False)

#preparing output and grabbing original labels
output = model.predict(test_set)
print(output)
classes = np.argmax(output, axis=1)
labels = test_set.labels
files = test_set.filenames
#going through all predicted labels and comparing them to the original
correct = 0
total = 0
for i in range(len(classes)):
    if labels[i] == classes[i]:
        correct += 1
    print("Real: " + str(labels[i]) + " Prediction: " + str(classes[i]))
    total += 1

print("Accuracy: ", correct/total)
