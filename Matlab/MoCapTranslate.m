% --- Executes on button press in pushbutton8.
%function pushbutton8_Callback(handles)
% hObject    handle to pushbutton8 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

load('data2.mat')
% open a file for writing
textFile = fopen('MoCapData.txt', 'w');

% print a title, followed by a blank line
fprintf(textFile, 'tree_021\n\n');

tree = eval('tree_021');

% Get info about data set
nSamples = length(tree.subject.frames.frame);
time=zeros(nSamples-2,1);
time_initial = tree.subject.frames.frame(3).ms; %first (non-calibration) time

L5S1Ind = 1;
L4L3Ind = 2;
L1T1 = 3;

RightC7ShoulderInd = 7;
RightShoulderInd = 8;
RightElbowInd = 9;
RightWristInd = 10;

LeftC7ShoulderInd = 11;
LeftShoulderInd = 12;
LeftElbowInd = 13;
LeftWristInd = 14;

RightHipInd = 15;
RightKneeInd = 16;
RightAnkleInd = 17;
RightBallFootInd = 18;

LeftHipInd = 19;
LeftKneeInd = 20;
LeftAnkleInd = 21;
LeftBallFootInd = 22;
     
for i=[1:nSamples-2]
  %note that the first two lines are calibration poses
  %actual data starts at 3rd line
  
  position = tree.subject.frames.frame(i+2).position;
  jointAngle = tree.subject.frames.frame(i+2).jointAngle;
  jointAngleXZY = tree.subject.frames.frame(i+2).jointAngleXZY;
    
  %find x,y,z global positions of each segment
  Pelvis(i,:)= position(1:3);
  %orient = tree.subject.frames.frame(i+2).orientation(1:4);
  %rpy = quat2rpy(orient);
  %Pelvis_or(i,:) = rpy;
  
  L5(i,:)= position(4:6);
  L3(i,:)= position(7:9);
  T12(i,:)= position(10:12);
  T8(i,:)= position(13:15);
  Neck(i,:)= position(16:18);
  Head(i,:)= position(19:21);
  Right_Shoulder(i,:)= position(22:24);
  Right_Upper_Arm(i,:)= position(25:27);
  Right_Forearm(i,:)= position(28:30);
  Right_Hand(i,:)= position(31:33);
  Left_Shoulder(i,:)= position(34:36);
  Left_Upper_Arm(i,:)= position(37:39);
  Left_Forearm(i,:)= position(40:42);
  Left_Hand(i,:)= position(43:45);
  Right_Upper_Leg(i,:)= position(46:48);
  Right_Lower_Leg(i,:)= position(49:51);
  Right_Foot(i,:)= position(52:54);
  Right_Toe(i,:)= position(55:57);
  Left_Upper_Leg(i,:)= position(58:60);
  Left_Lower_Leg(i,:)= position(61:63);
  Left_Foot(i,:)= position(64:66);
  Left_Toe(i,:)= position(67:69);
end

fprintf(textFile, '\n Right_Hand \n');
fprintf(textFile, '%f %f %f\n', Right_Hand);

fprintf(textFile, '\n Left_Hand \n');
fprintf(textFile, '%f %f %f\n', Left_Hand);

fprintf(textFile, '\n Right_Toe \n');
fprintf(textFile, '%f %f %f\n', Right_Toe);

fprintf(textFile, '\n Left_Toe \n');
fprintf(textFile, '%f %f %f\n', Left_Toe);

fclose(textFile);