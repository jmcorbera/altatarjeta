function InfoModel(data) {
    var self = this;

    self.IdCommerce = ko.observable(data.IdCommerce || '');
    self.CUIT = ko.observable(data.CUIT || '');
    self.Description = ko.observable(data.Description || '');
    self.Address = ko.observable(data.Address || '');
    self.LocationCode = ko.observable(data.LocationCode || '');
    self.Location = ko.observable(data.Location || '');
    self.Province = ko.observable(data.Province || '');
    self.ProvinceCode = ko.observable(data.ProvinceCode || '');
    self.IdSubsidiary = ko.observable(data.IdSubsidiary || '');

}


function InfoViewModel(app, dataModel) {
    var self = this;

    Sammy(function () {
        this.get('Home/Info', function () { this.app.runRoute('get', '#info') });
    });

    self.infoModel = ko.observable();

    self.LoadInfo = function () {
        //var url = UrlAction('Home', 'GetUserInformation');

        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: "Home/GetUserInformation",
            success: function (data) {
                if (data.success) {
                    self.infoModel(new InfoModel(data.list));
                }
                else {
                    //Utils.showErrorModal(data.error);
                }
            },
            error: function (xhr) {
                //Utils.showErrorModal(xhr.responseText);
            }
        });

    }

    self.LoadInfo();

    return self;
}

app.addViewModel({
    name: "Info",
    bindingMemberName: "info",
    factory: InfoViewModel
});