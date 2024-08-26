var viewer;

function launchViewer(urn) {
    var options = {
        // svf:
        env: 'AutodeskProduction',
        // svf2:
        //env: 'AutodeskProduction2',
        //api: 'streamingV2',
        getAccessToken: (onGetAccessToken) => {
            fetch('api/token')
                .then(response => response.json())
                .then(data => {
                    let accessToken = data["accessToken"];
                    let expireTimeSeconds = data["expiresIn"];
                    onGetAccessToken(accessToken, expireTimeSeconds);
                })
        }
    };

    var documentId = 'urn:' + urn;

    Autodesk.Viewing.Initializer(options, function() {
        var viewerDiv = document.getElementById('apsViewer');
        viewer = new Autodesk.Viewing.GuiViewer3D(viewerDiv);

        var startedCode = viewer.start();
        if (startedCode > 0) {
            console.error('Failed to create a Viewer: WebGL not supported.');
            return;
        }

        // Prevent using cached model
        Autodesk.Viewing.endpoint.HTTP_REQUEST_HEADERS['If-Modified-Since'] = 'Sat, 29 Oct 1994 19:43:31 GMT';

        Autodesk.Viewing.Document.load(documentId, onDocumentLoadSuccess, onDocumentLoadFailure);
    });
}

function onDocumentLoadSuccess(doc) {
    var defaultModel = doc.getRoot().getDefaultGeometry();
    viewer.loadDocumentNode(doc, defaultModel);
}

function onDocumentLoadFailure(viewerErrorCode) {
    console.error('onDocumentLoadFailure() - errorCode:' + viewerErrorCode);
}
