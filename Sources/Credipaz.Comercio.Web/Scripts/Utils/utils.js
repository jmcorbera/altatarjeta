(function (exports, undefined) {

    var Utils = {
        initViewModel: initViewModel,
        showErrorModal: showErrorModal,
        showInfoModal: showInfoModal,
        showConfirmModal: showConfirmModal,

        getItemById: getItemById
    };

    function getItemById(list, Id) {
        return ko.utils.arrayFirst(list, function (item) {
            return item.Id == Id;
        })
    }

    function initViewModel(node, model) {
        ko.cleanNode(node);
        ko.applyBindings(model, node);
    }

    function showErrorModal(message, param1) {
        var $modal = $("#errorModal");
        if (param1 == null)
            $modal.find("#error-modal-body").text(message);
        else {
            var str = message.replace("{0}", param1);
            $modal.find("#error-modal-body").text(str);
        }
        $modal.find(".accept").off('click');
        $modal.modal("show");
    };

    function showInfoModal(message) {
        var $modal = $("#infoModal");
        $modal.find("#info-modal-body").text(message);
        $modal.find(".accept").off('click');
        $modal.modal("show");
        return $modal;
    };

    function showConfirmModal(message) {
        var $modal = $("#confirmModal");
        $modal.find("#confirm-modal-body").text(message);
        $modal.find(".accept").off('click');
        $modal.modal("show");
        return $modal;
    };

    exports.Utils = Utils;

})(window);