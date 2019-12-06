arms = {}

String.prototype.format = function () {
    var args = (arguments.length === 1 && $.isArray(arguments[0])) ? arguments[0] : arguments;
    var formattedString = this;
    for (var i = 0; i < args.length; i++) {
        var pattern = new RegExp("\\{" + i + "\\}", "gm");
        formattedString = formattedString.replace(pattern, args[i]);
    }
    return formattedString;
};

arms.util = {
    ajaxGet: function (url, previousRequest, doSynchronousCall, successCallback, errorCallback, completeCallback) {
        // default values
        if (typeof doSynchronousCall !== 'boolean') { doSynchronousCall = false; }

        console.log('GET: ' + url);

        return $.ajax({
            type: 'get',
            url: url,
            data: '',
            contenttype: 'application/json; charset=utf-8',
            datatype: 'json',
            beforeSend: function () {
                // Abort any current request
                if (typeof previousRequest !== 'undefined' && previousRequest !== null && previousRequest.readyState !== 4) {
                    console.warn('ABORT: Aborting the following ajax request:');
                    console.warn(previousRequest);
                    previousRequest.abort();
                    console.warn('ABORT: Request aborted.');
                }
            },
            async: !doSynchronousCall,
            success: function (response) {
                if (typeof (successCallback) === 'function' && successCallback !== null) successCallback(response);
            },
            error: function (response) {
                if (typeof (errorCallback) === 'function' && errorCallback !== null) errorCallback(response);
            },
            complete: function (response) {
                if (typeof (completeCallback) === 'function' && completeCallback !== null) completeCallback(response);
            }
        });
    }
}

$('.call-async').click(function (e) {
    e.preventDefault();
    console.log('async call clicked');

    var that = $(this);

    var href = that.prop('href');
    console.log(href);

    var id = that.data('id');
    console.log(id);

    var url = '{0}?Id={1}'.format(href, encodeURIComponent(id));

    // Call the given url
    cockpit.page.home.operateRequest = arms.util.ajaxGet(url, cockpit.page.home.operateRequest, null,
        function () {
            console.log('ajax called');
        });
});

$('.async-delete').click(function (e) {
    e.preventDefault();
    console.log('async call clicked');

    var that = $(this);

    var href = that.prop('href');
    console.log(href);

    var type = that.data('type');

    switch (type) {
        case 'user':
            var userId = that.data('user-id');
            var courseId = that.data('course-id');

            var url = '{0}?userId={1}&courseId={2}'.format(href, encodeURIComponent(userId), encodeURIComponent(courseId));
            break;
        case 'lecture':

            var lectureId = that.data('lecture-id');
            console.log(userId);

            var url = '{0}?lectureId={1}'.format(href, encodeURIComponent(lectureId));
            console.log(url);

            break;
        default:
    }
    
    // Call the given url
    arms.util.ajaxGet(url, null, null,
        function () {
            window.location.reload();
        });
});

$('#addSupervisorModal, #addParticipantModal, #addLectureModal').on('hidden.bs.modal', function () {
    // Remove the content from the modal once the user closes it
    $('.result-body').html('');
});

$('.async-add').click(function (e) {
    e.preventDefault();
    console.log('async call clicked');

    var that = $(this);

    var href = that.prop('href');
    console.log('href = ' + href);
    var courseId = that.data('course-id');
    console.log('courseId = ' + courseId);

    var type = that.data('type');
    console.log('type = ' + type)
    var url;

    var modal = that.parent().parent();

    switch (type) {
        case 'supervisor':
            var email = modal.find('input[name="inputSupervisorEmail"]').val();
            url = '{0}?courseId={1}&email={2}'.format(href, encodeURIComponent(courseId), email);
            break;
        case 'participant':
            var email = modal.find('input[name="inputParticipantEmail"]').val();
            url = '{0}?courseId={1}&email={2}'.format(href, encodeURIComponent(courseId), encodeURIComponent(email));
            break;
        case 'lecture':
            var dateFrom = modal.find('input[name="dateFrom"]').val();
            var dateTo = modal.find('input[name="dateTo"]').val();
            url = '{0}?courseId={1}&dateFrom={2}&dateTo={3}'.format(href, encodeURIComponent(courseId), encodeURIComponent(dateFrom), encodeURIComponent(dateTo));
            break;
        default:
    }

    // Call the given url
    arms.util.ajaxGet(url, null, null,
        function () {
            window.location.reload();
        });
});