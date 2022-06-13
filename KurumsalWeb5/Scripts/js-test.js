function onScanSuccess(qrCodeMessage) {
            document.getElementById('result').innerHTML = '<span class="result">' + qrCodeMessage + '</span>';
            BarkodGonder(qrCodeMessage);

        };

        function BarkodGonder(qrCodeMessage) {
            var BarcodeNumber = qrCodeMessage;

            var categoryId = 1;
            var productname = "test";
            var quantity = 5;
            var Limit = 0;
            var Defaultquantity = 0;
            var UnitPrice = 0;

            let launcher = { BarcodeNumber, productname, categoryId, UnitPrice, quantity, Defaultquantity, Limit };
            alert(BarcodeNumber);

            $.ajax({
                type: "POST",
                url: "/api/ScanBarcode",
                contentType: 'application/json',
                dataType: "json",
                data: JSON.stringify(launcher), //"name="+name+"&lastName="+lastname+"&email="+email,

                success: function (response) {
                    console.log("başarı " + BarcodeNumber)
                    return
                    //console.log("Succes")
                },
                error: function (response) {
                    console.log("Hata " + BarcodeNumber)
                    return
                    //console.log("failure")
                }
            });
        }
        function onScanError(errorMessage) {
            //handle scan error
        }

        var html5QrcodeScanner = new Html5QrcodeScanner(
            "reader", { fps: 10, qrbox: 250 });
        html5QrcodeScanner.render(onScanSuccess, onScanError);