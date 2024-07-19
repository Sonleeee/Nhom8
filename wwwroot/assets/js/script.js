
function formatDate(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

function updateChildrenAges() {
    const childrenCount = parseInt(document.getElementById('children').value);
    const childrenAgesContainer = document.getElementById('children-ages');
    childrenAgesContainer.innerHTML = '';
    for (let i = 1; i <= childrenCount; i++) {
        const ageInput = `
                    <div class="form-group">
                        <label for="child${i}">Trẻ em ${i}</label>
                        <input type="number" class="form-control" id="child${i}" value="8">
                    </div>
                `;
        childrenAgesContainer.insertAdjacentHTML('beforeend', ageInput);
    }
}

document.addEventListener('DOMContentLoaded', function () {
    updateChildrenAges();

    // Xác nhận ngày
    document.getElementById('confirmDate').addEventListener('click', function ( event) {
        event.preventDefault();
        let checkInDateValue = document.getElementById('checkInDate').value;
        let checkOutDateValue = document.getElementById('checkOutDate').value;        

        const today = new Date();
        today.setHours(0, 0, 0, 0);

        const checkInDate = new Date(checkInDateValue);
        checkInDate.setHours(0, 0, 0, 0);

        if (checkInDate > today)
        {            
            const checkInDate = new Date(checkInDateValue);
            const checkOutDate = new Date(checkOutDateValue);

            let CheckInDateConvert = formatDate(checkInDate);
            let CheckOutDateConvert = formatDate(checkOutDate);

            console.log(CheckInDateConvert);
            console.log(CheckOutDateConvert);
            document.getElementById('datePicker').innerText = `${CheckInDateConvert} - ${CheckOutDateConvert}`;
        }           
    });

    const today = new Date();

    // Định dạng ngày theo định dạng yyyy-mm-dd
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // Tháng bắt đầu từ 0 nên cần +1
    const dd = String(today.getDate()).padStart(2, '0');
    const formattedToday = `${yyyy}-${mm}-${dd}`;

    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    const yyyyTomorrow = tomorrow.getFullYear();
    const mmTomorrow = String(tomorrow.getMonth() + 1).padStart(2, '0');
    const ddTomorrow = String(tomorrow.getDate()).padStart(2, '0');
    const formattedTomorrow = `${yyyyTomorrow}-${mmTomorrow}-${ddTomorrow}`;

    // Set giá trị cho các trường input
    document.getElementById('checkInDate').value = formattedToday;
    document.getElementById('checkOutDate').value = formattedTomorrow;

    // - số lượng người lớn
    document.getElementById('increase-adults').addEventListener('click', function (event) {
        event.preventDefault();
        let adults = parseInt(document.getElementById('adults').value);
        adults += 1;
        document.getElementById('adults').value = adults;
    });

    document.getElementById('decrease-adults').addEventListener('click', function (event) {
        event.preventDefault();
        let adults = parseInt(document.getElementById('adults').value);
        if (adults > 1) {
            adults -= 1;
        }
        document.getElementById('adults').value = adults;
    });

    //- số lượng trẻ em

    document.getElementById('increase-children').addEventListener('click', function (event) {
        event.preventDefault();
        let children = parseInt(document.getElementById('children').value);
        children += 1;
        document.getElementById('children').value = children;
        updateChildrenAges();
    });

    document.getElementById('decrease-children').addEventListener('click', function (event) {
        event.preventDefault();
        let children = parseInt(document.getElementById('children').value);
        if (children > 1) {
            children -= 1;
        }
        document.getElementById('children').value = children;
        updateChildrenAges();
    });

    // số lượng phòng

    document.getElementById('increase-room').addEventListener('click', function (event) {
        event.preventDefault();
        let room = parseInt(document.getElementById('rooms').value);
        room += 1;
        document.getElementById('rooms').value = room;
    });

    document.getElementById('decrease-room').addEventListener('click', function (event) {
        event.preventDefault();
        let room = parseInt(document.getElementById('rooms').value);
        if (room > 1) {
            room -= 1;
        }
        document.getElementById('rooms').value = room;
    });

    // Xác nhận số lượng người lớn, trẻ em và phòng
    document.getElementById('confirmGuest').addEventListener('click', function (event) {
        event.preventDefault();
        let adults = document.getElementById('adults').value;
        let children = document.getElementById('children').value;
        let rooms = document.getElementById('rooms').value;

        document.getElementById('guestPicker').innerText = `${adults} người lớn - ${children} trẻ em - ${rooms} phòng`;
    });  


    // Prevent dropdown from closing on click inside
    $('.dropdown-menu').on('click', function (event) {
        event.stopPropagation();
    });

    // Manually toggle dropdown to control it
    $('#dropdownMenuButton').on('click', function () {
        $('.dropdown-menu').toggleClass('show');
    });

    $(document).on('click', function (event) {
        if (!$(event.target).closest('.dropdown').length) {
            $('.dropdown-menu').removeClass('show');
        }
    });

    
});


// index - begin

document.addEventListener("DOMContentLoaded", function () {
    var navItems = document.querySelectorAll(".nav-itemm");

    navItems.forEach(function (item) {
        item.addEventListener("click", function () {
            // Loại bỏ lớp nav-active từ tất cả các nav-item
            navItems.forEach(function (nav) {
                nav.classList.remove("nav-active");
            });

            // Thêm lớp nav-active vào nav-item được bấm
            this.classList.add("nav-active");
        });
    });
});


// index - end

// setroom - begin

const radioKhachLuuTru = document.getElementById('user-gues');
    const radioKhachThamQuan = document.getElementById('no-user-gues');
    const formNhapTen = document.getElementById('formNhapTen');

    radioKhachLuuTru.addEventListener('click', function() {
      formNhapTen.classList.add('d-none');
    });

    radioKhachThamQuan.addEventListener('click', function() {
      formNhapTen.classList.remove('d-none');
    });

// setroom - end
