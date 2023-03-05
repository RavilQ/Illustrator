
$(document).on("click", ".slider-delete-btn", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    Swal.fire({
        text: "Are you sure?!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(url)
                .then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        ).then(() => window.location.reload())
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Slider not found!',
                            text: 'Something went wrong!',
                        })
                    }
                })
        }
    })
})
