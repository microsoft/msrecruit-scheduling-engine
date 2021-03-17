function trigger() {
    var collection = getContext().getCollection();
    var request = getContext().getRequest();
    var document = request.getBody();
    var documentLink = collection.getAltLink() + '/docs/' + 'autoNumbers';

    var documentType = document["Type"];

    if (documentType != null) {
        collection.readDocument(documentLink, {}, function (err, autoNumberDoc, options) {
            if (err) {
                if (err.number == 404) {
                    var newDoc = {};
                    newDoc["id"] = "autoNumbers";
                    newDoc[documentType] = 1;
                    newDoc['Type'] = documentType;

                    collection.createDocument(collection.getAltLink(), newDoc, {}, function (err, doc, options) {
                        document[documentType + "Id"] = "1";
                        request.setBody(document);
                        if (err) throw new Error(err.number, "Failed to create autoNumbers document " + err.message);
                    });
                }
                else throw err;
            } else {
                if (autoNumberDoc[documentType] == null) {
                    autoNumberDoc[documentType] = 1;
                } else {
                    autoNumberDoc[documentType] += 1;
                }

                collection.replaceDocument(documentLink, autoNumberDoc, function (err, doc, options) {
                    if (err) throw new Error(err.number, "Failed to replace autoNumbers document " + err.message);

                    document[documentType + "Id"] = autoNumberDoc[documentType].toString();

                    request.setBody(document);
                });
            }
        });
    }
}
