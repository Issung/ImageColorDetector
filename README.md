# ImageColorDetector
Quick and dirty (filthy) script I made to detect the average color of all images in a directory and then check that against a threshold and then move the files within the threshold

Made in ~15mins for my 67,000 images that needed filtering. Detects all images that are almost completely black or almost completely white, and moves them into a folder called "Detected".
Multi threaded for speed, did about 67,000 in under 15ish seconds.
Nasty code.
Doesn't take any cli args unfortunately (takes your desired dir as readline).
Could easily be changed to do something else in future.

