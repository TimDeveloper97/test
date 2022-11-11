import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';

import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Injectable({
    providedIn: 'root'
})
export class CheckSpecialCharacter {
    checkCode(code) {
        //kiểm tra ký tự đặc việt trong Mã
        var index1 = code.indexOf("*");
        var index2 = code.indexOf("{");
        var index3 = code.indexOf("}");
        var index4 = code.indexOf("!");
        var index5 = code.indexOf("^");
        var index6 = code.indexOf("<");
        var index7 = code.indexOf(">");
        var index8 = code.indexOf("?");
        var index9 = code.indexOf("|");
        var index10 = code.indexOf(",");
        var index11 = code.indexOf("_");
        var index12 = code.indexOf(" ");

        var validCode = true;
        if (index1 != -1 || index2 != -1 || index3 != -1 || index4 != -1 || index5 != -1 || index6 != -1 || index7 != -1 || index8 != -1 || index9 != -1
            || index10 != -1 || index11 != -1 || index12 != -1) {
            validCode = false;
        }
        return validCode;
    }

    checkCodeMaterial(code) {
        //kiểm tra ký tự đặc việt trong Mã
        var index1 = code.indexOf("*");
        var index2 = code.indexOf("{");
        var index3 = code.indexOf("}");
        var index4 = code.indexOf("!");
        var index5 = code.indexOf("^");
        var index6 = code.indexOf("<");
        var index7 = code.indexOf(">");
        var index8 = code.indexOf("?");
        var index9 = code.indexOf("|");
        var index11 = code.indexOf("_");
        var index12 = code.indexOf(" ");

        var validCode = true;
        if (index1 != -1 || index2 != -1 || index3 != -1 || index4 != -1 || index5 != -1 || index6 != -1 || index7 != -1 || index8 != -1 || index9 != -1 
            || index11 != -1 || index12 != -1) {
            validCode = false;
        }
        return validCode;
    }

    checkUnicode(code) {
        var codeLowerCase = code.toLowerCase();
        //kiểm tra ký tự đặc việt trong Mã
        var index13 = codeLowerCase.indexOf("á");
        var index14 = codeLowerCase.indexOf("à");
        var index15 = codeLowerCase.indexOf("ã");
        var index16 = codeLowerCase.indexOf("ạ");
        var index17 = codeLowerCase.indexOf("ả");
        var index18 = codeLowerCase.indexOf("ă");
        var index19 = codeLowerCase.indexOf("ắ");
        var index20 = codeLowerCase.indexOf("ằ");
        var index21 = codeLowerCase.indexOf("ẵ");
        var index22 = codeLowerCase.indexOf("ặ");
        var index23 = codeLowerCase.indexOf("ẳ");
        var index24 = codeLowerCase.indexOf("â");
        var index25 = codeLowerCase.indexOf("ấ");
        var index26 = codeLowerCase.indexOf("ầ");
        var index27 = codeLowerCase.indexOf("ẫ");
        var index28 = codeLowerCase.indexOf("ậ");
        var index29 = codeLowerCase.indexOf("ẩ");
        var index30 = codeLowerCase.indexOf("é");
        var index31 = codeLowerCase.indexOf("è");
        var index32 = codeLowerCase.indexOf("ẽ");
        var index33 = codeLowerCase.indexOf("ẹ");
        var index34 = codeLowerCase.indexOf("ẻ");
        var index35 = codeLowerCase.indexOf("ê");
        var index36 = codeLowerCase.indexOf("ế");
        var index37 = codeLowerCase.indexOf("ề");
        var index38 = codeLowerCase.indexOf("ễ");
        var index39 = codeLowerCase.indexOf("ệ");
        var index40 = codeLowerCase.indexOf("ể");
        var index41 = codeLowerCase.indexOf("í");
        var index42 = codeLowerCase.indexOf("ì");
        var index43 = codeLowerCase.indexOf("ĩ");
        var index44 = codeLowerCase.indexOf("ị");
        var index45 = codeLowerCase.indexOf("ỉ");
        var index46 = codeLowerCase.indexOf("ó");
        var index47 = codeLowerCase.indexOf("ò");
        var index48 = codeLowerCase.indexOf("õ");
        var index49 = codeLowerCase.indexOf("ọ");
        var index50 = codeLowerCase.indexOf("ỏ");
        var index51 = codeLowerCase.indexOf("ô");
        var index52 = codeLowerCase.indexOf("ố");
        var index53 = codeLowerCase.indexOf("ồ");
        var index54 = codeLowerCase.indexOf("ỗ");
        var index55 = codeLowerCase.indexOf("ộ");
        var index56 = codeLowerCase.indexOf("ổ");
        var index57 = codeLowerCase.indexOf("ơ");
        var index58 = codeLowerCase.indexOf("ớ");
        var index59 = codeLowerCase.indexOf("ờ");
        var index60 = codeLowerCase.indexOf("ỡ");
        var index61 = codeLowerCase.indexOf("ợ");
        var index62 = codeLowerCase.indexOf("ở");
        var index63 = codeLowerCase.indexOf("ú");
        var index64 = codeLowerCase.indexOf("ù");
        var index65 = codeLowerCase.indexOf("ũ");
        var index66 = codeLowerCase.indexOf("ụ");
        var index67 = codeLowerCase.indexOf("ủ");
        var index68 = codeLowerCase.indexOf("ư");
        var index69 = codeLowerCase.indexOf("ứ");
        var index70 = codeLowerCase.indexOf("ừ");
        var index71 = codeLowerCase.indexOf("ữ");
        var index72 = codeLowerCase.indexOf("ự");
        var index73 = codeLowerCase.indexOf("ử");
        var index74 = codeLowerCase.indexOf("ý");
        var index75 = codeLowerCase.indexOf("ỳ");
        var index76 = codeLowerCase.indexOf("ỹ");
        var index77 = codeLowerCase.indexOf("ỵ");
        var index78 = codeLowerCase.indexOf("ỷ");

        var validCode = true;
        if (index13 != -1 || index14 != -1 || index15 != -1 || index16 != -1 || index17 != -1 || index18 != -1 || index19 != -1 || index20 != -1 || index21 != -1 || index22 != -1 || index23 != -1 || index24 != -1 || index25 != -1 || index26 != -1 || index27 != -1 || index28 != -1 || index29 != -1 || index30 != -1 || index31 != -1 || index32 != -1 || index33 != -1 || index34 != -1 || index35 != -1 || index36 != -1 || index37 != -1 || index38 != -1 || index39 != -1 || index40 != -1 || index41 != -1 || index42 != -1 || index43 != -1 || index44 != -1 || index45 != -1 || index46 != -1 || index47 != -1 || index48 != -1 || index49 != -1 || index50 != -1 
            || index51 != -1|| index52 != -1|| index53 != -1|| index54 != -1|| index55 != -1|| index56 != -1|| index57 != -1|| index58 != -1|| index59 != -1|| index60 != -1|| index61 != -1|| index62 != -1|| index63 != -1|| index64 != -1
            || index65 != -1|| index66 != -1|| index67 != -1|| index68 != -1|| index69 != -1|| index70 != -1|| index71 != -1|| index72 != -1
            || index73 != -1|| index74 != -1|| index75 != -1|| index76 != -1|| index77 != -1|| index78 != -1) {
            validCode = false;
        }
        return validCode;
    }
}