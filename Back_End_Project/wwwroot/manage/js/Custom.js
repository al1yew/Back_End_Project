

$(document).ready(function () {
    $(document).on('click', '.deleteBtn', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                let url = $(this).attr('href');

                fetch(url)
                    .then(res => res.text())
                    .then(data => { $('.tblContent').html(data) });

                Swal.fire(
                    'Deleted!',
                    '',
                    'success'
                )
            }
        })
    })

    $(document).on('click', '.restoreBtn', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, restore it!'
        }).then((result) => {
            if (result.isConfirmed) {

                let url = $(this).attr('href');

                fetch(url)
                    .then(res => {
                        if (res.status == 400) {
                            //alert("mumkun olmadi");
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: 'Something went wrong!',
                                footer: '<a href="">Why do I have this issue?</a>'
                            })
                        } else {
                            Swal.fire(
                                'Restored!',
                                '',
                                'success'
                            )

                            return res.text()

                        }

                    })
                    .then(data => { $('.tblContent').html(data) });
            }
        })
    })


    $(document).on('click', '.deleteproductimage', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.productimages').html(data);
            })
    });



    if ($('.isMaininput').is(":checked")) {
        $('.imagecontainer').removeClass('d-none');
        $('.parentcontainer').addClass('d-none');
    } else {
        $('.imagecontainer').addClass('d-none');
        $('.parentcontainer').removeClass('d-none');
    }

    $(document).on('change', '.isMaininput', function () {
        console.log($(this).is(":checked"))
        if ($(this).is(":checked")) {
            $('.imagecontainer').removeClass('d-none');
            $('.parentcontainer').addClass('d-none');
        } else {
            $('.imagecontainer').addClass('d-none');
            $('.parentcontainer').removeClass('d-none');
        }
    })


    $(document).on('click', '.Updatebtn', function (e) {
        e.preventDefault();
        console.log("test");
        $(this).parent().addClass('d-none');
        $(this).parent().next().removeClass('d-none');
    })

    $(document).on('click', '.settingUpdatebtn', function (e) {
        e.preventDefault();

        let url = $('.updateForm').attr('action');

        let key = $(this).prev().attr('name');
        let value = $(this).prev().val();
        console.log(key)
        console.log(value)

        let bodyObj = {
            key: key,
            value: value
        }

        fetch(url, {
            method: 'Post',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(bodyObj)
        })
            .then(res => res.text())
            .then(data => {
                $('.settingContainer').html(data)
            })
    })




    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    if ($('#successInput').val().length) {
        toastr["success"]($('#successInput').val(), $('#successInput').val().split(' ')[0])
    }
})