function saveVideo( datasetName )

if ~exist( datasetName, 'dir' )
    error('Specified Folder do not exist.');
end

curPath = pwd;
cd( datasetName );

vidObj = VideoWriter( [ datasetName '.avi' ]);
open(vidObj);

images = dir( '*.png' );

for i = 0 : length( images ) - 1
    currFrame = imread( sprintf( '%010d.png', i ) );
    writeVideo(vidObj,currFrame);
end

close(vidObj);

cd( curPath );

end

