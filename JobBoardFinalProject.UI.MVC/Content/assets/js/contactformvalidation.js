function validateForm(event) {

    //Gather all HTML controls into a collection instead of creating a separate variable for each
    let controls = document.getElementsByClassName('form-control');

    console.log(controls);

    let validationMessages = document.getElementsByClassName("text-danger");
    console.log(validationMessages);

    //Regular Expression for Email - regexlib.com
    //Add // in parens and paste expression between slashes. Leave slashes.
    let rxpEmail = new RegExp(/^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/);

    //Check the length of ALL controls - if any have NOT been filled out by the user, stop the form from submitting
    if (controls["name"].value.length == 0 || controls["email"].value.length == 0 || controls["subject"].value.length == 0 || controls["message"].value.length == 0 || !rxpEmail.test(controls["email"].value)) {

        //Stop the form from submitting
        event.preventDefault();

        //Check individual controls and display error message if needed
        //Name Validation
        //length
        if (controls["name"].value.length == 0) {
            validationMessages["rfvName"].textContent = "* JSName is required";
        }
        else {
            validationMessages["rfvName"].textContent = "";
        }

        //Email Validation
        //length
        if (controls["email"].value.length == 0) {
            validationMessages["rfvEmail"].textContent = "* Email is required";
        }
        else {
            validationMessages["rfvEmail"].textContent = "";
        }
        //regex
        if (!rxpEmail.test(controls["email"].value) && controls["email"].value.length > 0) {
            validationMessages["rfvEmail"].textContent = "*Please provide a valid email address";
        }

        if (rxpEmail.test(controls["email"].value) && controls["email"].value.length > 0) {
            validationMessages["rfvEmail"].textContent = "";
        }

        //Subject Validation
        if (controls["subject"].value.length == 0) {
            validationMessages["rfvSubject"].textContent = "* Subject is required";
        }
        else if (controls["subject"].value.length > 25) {
            validationMessages["rfvSubject"].textContent = "*Subject must be 25 characters or less";
        }
        else {
            validationMessages["rfvSubject"].textContent = "";
        }


        //Message Validation
        if (controls["message"].value.length == 0) {
            validationMessages["rfvMessage"].textContent = "* Message is required";
        }
        else {
            validationMessages["rfvMessage"].textContent = "";
        }

    }

}