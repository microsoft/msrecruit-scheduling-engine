function trigger() {
    var collection = getContext().getCollection();
    var request = getContext().getRequest();
    var document = request.getBody();
    var documentLink = collection.getAltLink() + '/docs/' + 'autoNumbers';

    var tenantId = document["tenantId"];

    if (tenantId != null) {
        collection.readDocument(documentLink, {}, function (err, autoNumberDoc, options) {
            if (err) {
                if (err.number == 404) {
                    var newDoc = {};
                    newDoc["id"] = "autoNumbers";
                    newDoc["autoNumber"] = 1;
                    newDoc['tenantId'] = tenantId;

                    collection.createDocument(collection.getAltLink(), newDoc, {}, function (err, doc, options) {
                        document["autoNumber"] = "1";
                        request.setBody(document);
                        if (err) throw new Error(err.number, "Failed to create autoNumbers document " + err.message);
                    });
                }
                else throw err;
            } else {
                if (autoNumberDoc["autoNumber"] == null) {
                    autoNumberDoc["autoNumber"] = 1;
                } else {
                    autoNumberDoc["autoNumber"] += 1;
                }

                collection.replaceDocument(documentLink, autoNumberDoc, function (err, doc, options) {
                    if (err) throw new Error(err.number, "Failed to replace autoNumbers document " + err.message);

                    document["autoNumber"] = autoNumberDoc["autoNumber"].toString();

                    request.setBody(document);
                });
            }
        });
    }
}