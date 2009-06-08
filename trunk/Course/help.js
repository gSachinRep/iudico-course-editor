function textBoxFocus(textBox){
    if (textBox.value == textBox.defValue){
        textBox.value = '';
        textBox.style.fontStyle = 'normal';
    }
}

function textBoxBlur(textBox){
    if (textBox.value == ''){
        textBox.value =  textBox.defValue;
        textBox.style.fontStyle = 'italic';
    }
}

function textBoxInit(textBox, defValue){
    textBox.defValue = defValue;
    textBoxBlur(textBox);
}