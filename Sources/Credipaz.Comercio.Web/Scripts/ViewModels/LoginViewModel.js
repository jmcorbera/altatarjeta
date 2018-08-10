function LoginViewModel() {
    var self = this;

    self.idUser = ko.observable().extend({ required: { message: window.messages.MessageRequired } });
    self.password = ko.observable().extend({ required: { message: window.messages.MessageRequired } });

    self.errors = ko.validation.group(self);

    self.SubmitLogin = function () {
        if (this.errors().length === 0) {
            this.errors.showAllMessages();

            try {
                var captcharesponse = grecaptcha.getResponse();

                var form = $('#__AjaxAntiForgeryForm');
                var token = $('input[name="__RequestVerificationToken"]', form).val();

                var LoginViewModel = {
                    IdUser: self.idUser(),
                    Password: self.password()
                }

                $.ajax({
                    url: 'ValidateUser',
                    type: "POST",
                    data: { __RequestVerificationToken: token, model: LoginViewModel, captcharesponse: grecaptcha.getResponse() },
                    success: function (data) {
                        if (data.success) {
                            window.location.href = data.url;
                        }
                        else {

                            Utils.showErrorModal(data.error);
                        }
                    },
                    error: function (xhr) {
                        Utils.showErrorModal(xhr.responseText);
                    },
                });

            }
            catch (err) {
                Utils.showErrorModal(err);
            }
        }
        else {
            Utils.showInfoModal(window.messages.CheckData);
            this.errors.showAllMessages();
        }
    }

}

$(function () {

    ko.validation.rules.pattern.message = 'Invalid.';

    ko.validation.init({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null
    }, true);

    ko.bindingHandlers.enterkey = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {

            var inputSelector = 'input,textarea,select';

            $(document).on('keypress', inputSelector, function (e) {
                var allBindings = allBindingsAccessor();
                $(element).on('keypress', 'input, textarea, select', function (e) {
                    var keyCode = e.which || e.keyCode;
                    if (keyCode !== 13) {
                        return true;
                    }

                    return false;
                });
            });
        }
    };

    var model = new LoginViewModel();
    var node = $("#content")[0];

    Utils.initViewModel(node, model);

})