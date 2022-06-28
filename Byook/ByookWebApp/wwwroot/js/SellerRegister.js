let validations = {
    isCheckBusinessNumber: false,
    isCheckPasswordCase1: false,
    isCheckPasswordCase2: false,
    isCheckPasswordCase3: false,
    isCheckPasswordConfirm: false,
    isCheckAddress: false,
    isCheckName: false,
    isCheckPhoneNumber: false
};

$("#findAddress").click(function() {
    new daum.Postcode({
        oncomplete: function(data) {

            let address = "";

            if(data.userSelectedType === "R") {
                address = data["roadAddress"];
            } else {
                address = data["jibunAddress"];
            }

            $("#address").attr("value", address);

            let otherAddress = $("#otherAddress");

            otherAddress.css("display", "block");
            otherAddress.focus();

            validations.isCheckAddress = true;
        }
    }).open();
});

$("#findBusinessNumber").click(function() {
    let businessNumber = $("#businessNumber");
    
    let data = {
        "b_no": [businessNumber.val()]
    };

    const serviceKey = "EH8ytX4Y%2BOJHqA4Usvxn9bZ62Pf2Ib2yS%2FOXoBFN7nvIUzzyF9wlFrNsMA7dxHb3AQR0Hfbga5ViBwB1S01kJA%3D%3D";

    $.ajax({
        url: "https://api.odcloud.kr/api/nts-businessman/v1/status?serviceKey=" + serviceKey,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        accept: "application/json",
        success: function(response) {
            const businessStatus = response.data[0]["b_stt"];
            
            if(businessStatus === "") {

                showModal("국세청에 등록되지 않은 사업자입니다.");

                validations.isCheckBusinessNumber = false;

                return;
            }

            IsRegistered(businessNumber.value);
        },
        error: function(error) {
            console.log(error.responseText);
        }
    });
});

$("#password").focus(function() {
    const guide = $("#td_password .guide");
    
    if(guide.css("display") === "none") {
        guide.css("display", "block");
    }
});

const password = $("#password");
const spans = $("#td_password .guide span");

const regex1 = /^(?=.*[a-zA-Z])(?=.*[!@#$%^*+=-])[\S]{10,}$/;
const regex2 = /^(?=.*[!@#$%^*+=-])(?=.*[0-9])[\S]{10,}$/;
const regex3 = /^(?=.*[0-9])(?=.*[a-zA-Z])[\S]{10,}$/;
const regex4 = /(\d)\1\1/;

password.keyup(function() {
    const value = password.val()

    if(value.length >= 10) {
        removeBadAndAddGood(spans[0]);
        validations.isCheckPasswordCase1 = true;
    } else {
        removeGoodAndAddBad(spans[0]);
        validations.isCheckPasswordCase1 = false;
    }

    if(regex1.test(value) || regex2.test(value) || regex3.test(value)) {
        removeBadAndAddGood(spans[1]);
        validations.isCheckPasswordCase2 = true;
    } else {
        removeGoodAndAddBad(spans[1]);
        validations.isCheckPasswordCase2 = false;
    }
    
    if(!regex4.test(value)) {
        removeBadAndAddGood(spans[2]);
        validations.isCheckPasswordCase3 = true;
    } else {
        removeGoodAndAddBad(spans[2]);
        validations.isCheckPasswordCase3 = false;
    }
});

function removeBadAndAddGood(span) {
    if(span.classList.contains("bad")) {
        span.classList.remove("bad");
    }

    if(!span.classList.contains("good")) {
        span.classList.add("good");
    }
}

function removeGoodAndAddBad(span) {
    if(span.classList.contains("good")) {
        span.classList.remove("good");
    }

    if(!span.classList.contains("bad")) {
        span.classList.add("bad");
    }
}

const passwordConfirm = $("#passwordConfirm");

passwordConfirm.keyup(function() {
    const span = $("#td_passwordConfirm .guide span");

    if(password.val() === passwordConfirm.val()) {
        removeBadAndAddGood(span[0]);
        validations.isCheckPasswordConfirm = true;
    } else {
        removeGoodAndAddBad(span[0]);
        validations.isCheckPasswordConfirm = false;
    }
});

passwordConfirm.focus(function() {
    const guide = $("#td_passwordConfirm .guide");

    if(guide.css("display") === "none") {
        guide.css("display", "block");
    }
});

$(".btnRegister").click(function(event) {
    let isAllTrue = true;
    let validationName = "";

    HasName();
    HasPhoneNumber();

    for(let [key, value] of Object.entries(validations)){
        if (!value) {
            validationName = key;
            isAllTrue = value;

            break;
        }
    }

    if(isAllTrue){
        return;
    }

    event.preventDefault();

    let message = "";
    
    switch (validationName) {
        case "isCheckBusinessNumber":
            message = "사업자등록번호 중복 확인을 해주세요.";

            break;
        case "isCheckPasswordCase1":
        case "isCheckPasswordCase2":
        case "isCheckPasswordCase3":
            message = "패스워드 조건을 만족해주세요.";

            password.focus();

            break;
        case "isCheckPasswordConfirm":
            message = "같은 패스워드를 입력해주세요.";

            passwordConfirm.focus();
            
            break;
        default:
            break;
    }

    console.dir(validations);

    showModal(message);
});

function IsRegistered(businessNumber) {
    $.ajax({
        url: "/api/member/sellers",
        contentType: "application/json",
        accept: "application/text",
        data: {
            businessNumber
        },
        success: function(response) {
            if(response) {
                showModal("이미 가입한 사업자입니다.");

                validations.isCheckBusinessNumber = false;

                return;
            }

            validations.isCheckBusinessNumber = true;

            showModal("등록 가능한 사업자번호입니다.");
        },
        error: function(error) {
            console.dir(error);
        }
    })
}

function HasName() {
    validations.isCheckName = $("#name").val() !== "";
}

function HasPhoneNumber() {
    validations.isCheckPhoneNumber = $("#phoneNumber").val() !== "";
}
