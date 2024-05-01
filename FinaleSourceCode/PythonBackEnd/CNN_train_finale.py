from keras.models import Sequential
from keras.layers import Dense, MaxPooling2D, Conv2D, Flatten
from keras.preprocessing.image import ImageDataGenerator

#Image, Batch, epochs, and input size for the model
image_size = (300, 225)
batch_size = 10
epochs = 50
input_shape = (300, 225, 3)

#creating training data, not splitting into validation yet because I think its causing issues with training
train_datagen = ImageDataGenerator(rescale=1.0/255, validation_split=0.1, rotation_range=90)
train_set = train_datagen.flow_from_directory(directory='WSU_Images', target_size=image_size, batch_size=batch_size, class_mode='categorical', subset="training")
validation_set = train_datagen.flow_from_directory(directory='WSU_Images', target_size=image_size, batch_size=batch_size, class_mode='categorical', subset="validation")

num_classes = train_set.num_classes

#model architecture
model = Sequential([
    Conv2D(32, 3, padding='same', activation='relu', input_shape=input_shape),
    MaxPooling2D(),
    Conv2D(128, 5, padding='same', activation='relu'),
    MaxPooling2D(),
    Conv2D(256, 3, padding='same', activation='relu'),
    MaxPooling2D(),
    Conv2D(128, 5, padding='same', activation='relu'),
    MaxPooling2D(),
    Conv2D(32, 3, padding='same', activation='relu'),
    MaxPooling2D(),
    Flatten(),
    Dense(num_classes, activation='softmax')
])

#Model information, preparation, training, and saving
model.summary()
model.compile(loss="categorical_crossentropy", optimizer="adam", metrics=['accuracy'])
model.fit(train_set, epochs=epochs, validation_data=validation_set, validation_split=0.1)
model.save('armas_gigas-2.h5', include_optimizer=True)
