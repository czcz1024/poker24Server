var rules = {};

rules.allow= function(arr) {
    return rules.isBoom(arr) || rules.isPair(arr) || rules.isSingle(arr) || rules.isSeq(arr);
}

rules.isBoom = function(arr) {
    return arr.length > 2 && rules.allSame(arr);
};

rules.isPair = function(arr) {
    return arr.length == 2 && rules.allSame(arr);
};

rules.isSingle = function (arr) { return arr.length == 1; };

rules.isSeq = function (arr) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] >14 || arr[i]<1) {
            return false;
        }
    }
    var cha = [];
    var sort = arr.sort(function(a, b) { return a - b; });
    for (var i = 0; i < sort.length - 1; i++) {
        cha.push(sort[i + 1] - sort[i]);
    }
    return rules.allSame(cha);
};

rules.allSame = function(arr) {
    if (arr.length < 1) return false;
    var v1 = arr[0];
    for (var i = 1; i < arr.length; i++) {
        if (arr[i] != v1)return false;
    }
    return true;
};

rules.isBiger= function(last, arr) {
    if (rules.isBoom(last)) {
        if (rules.isBoom(arr)) {
            if (arr.length > last.length) return true;

            if (arr[0] > last[0]) return true;
        }
        if (rules.isPair(arr)) {
            if (arr[0] == 4) return true;
        }
    }

    if (rules.isPair(last)) {
        if (rules.isBoom(arr)) return true;
        if (rules.isPair(arr)) {
            if (arr[0] > last[0]) return true;
        }
    }
    if (rules.isSingle(last)) {
        if (rules.isBoom(arr)) return true;
        if (rules.isSingle(arr)) {
            if (arr[0] > last[0]) return true;
        }
    }
    if (rules.isSeq(last)) {
        if (rules.isBoom(arr)) return true;
        if (rules.isSeq(arr)) {
            if (arr.length == last.length) {
                if (arr[0] > last[0]) return true;
            }
        }
    }


    return false;
}



rules.count= function(arr, v) {
    var cnt = 0;
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == v)cnt++;
    }
    return cnt;
}

rules.may= function(arr) {
    var dw = rules.count(arr, 22);
    var xw = rules.count(arr, 21);
    if (dw == 0 && xw == 0) {
        return rules.mayWithoutW(arr);
    }
    return rules.mayW(arr);
}

rules.mayWithoutW = function (arr) {
    if (rules.allSame(arr)) return [arr];//boom
    
    var seq = rules.twoAs2(arr).sort(function (a, b) { return a - b; });
    var cntA = rules.count(seq, 14);
    if (cntA == 0) return [seq];//no a

    var r = [];
    if (rules.isSeq(seq)) {
        r.push(seq);
    }
    var replacedA = seq;
    for (var i = 0; i < cntA; i++) {
        replacedA = rules.replaceFirstAas1(replacedA);
        if (rules.isSeq(replacedA)) {
            r.push(replacedA);
        }
    }
    return r;
}

rules.mayW = function (arr) {
    var dwcnt = rules.count(arr, 22);
    var xwcnt = rules.count(arr, 21);
    var rest = rules.restOfw(arr);
    if (rest.length == 0) {
        return rules.onlyW(arr);
    }
    if (rest.length == 1) {
        return rules.wAnd1(rest[0],dwcnt+xwcnt);
    }

    if (rules.allSame(rest)) {
        //w->boom
        var num = rest[0];
        for (var i = 0; i < dwcnt + xwcnt; i++) {
            rest.push(num);
        }
        return [rest];
    }
    return rules.wInSeq(rest, dwcnt + xwcnt);
}

rules.wAnd1 = function (num, wcnt) {
    var r = [];
    //boom
    var boom = [num];
    for (var i = 0; i < wcnt; i++) {
        boom.push(num);
    }
    r.push(boom);

    var n = num;
    if (n == 15) n = 2;

    var seq = rules.getWseq1(num, wcnt);
    for (var i = 0; i < seq.length; i++) {
        r.push(seq[i]);
    }

    if (n == 14) {
        var seq1 = rules.getWseq1(1, wcnt);
        for (var i = 0; i < seq1.length; i++) {
            r.push(seq1[i]);
        }
    }

    return r;
}

rules.getWseq1 = function (num, wcnt) {
    var r = [];
    var min = num - wcnt;
    var max = num + wcnt;
    var width = [];//全长
    var needcnt = wcnt + 1;
    for (var i = min; i <= max; i++) {
        width.push(i);
    }
    for (var i = 0; i < width.length - needcnt+1; i++) {
        var n = [];
        for (var j = 0; j < needcnt; j++) {
            n.push(width[j+i]);
        }
        
        if (rules.isSeq(n)) {
            r.push(n);
        }
    }
    return r;
}

rules.wInSeq = function (arr, wcnt) {
    console.log("w in seq");
}

rules.onlyW = function (arr) {
    var dwcnt = rules.count(arr, 22);
    var xwcnt = rules.count(arr, 21);
    if (dwcnt == 0 || xwcnt==0) {
        return [arr];
    } else {
        var r = [];
        for (var i = 0; i < dwcnt + xwcnt; i++) {
            r.push(21);
        }
        return [r];
    }
}

rules.restOfw = function (arr) {
    var r = [];
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] < 20) {
            r.push(arr[i]);
        }
    }
    return r;
}

rules.replaceFirstAas1 = function (arr) {
    var r = [];
    var replaced = false;
    for (var i = 0; i < arr.length; i++) {
        if (!replaced) {
            if (arr[i] == 14) {
                replaced = true;
                r.push(1);
            } else {
                r.push(arr[i]);
            }
        } else {
            r.push(arr[i]);
        }
    }
    return r;
}


rules.except= function(arr, exp) {
    var r = [];
    for (var i = 0; i < arr.length; i++) {
        var no = false;
        for (var j = 0; j < exp.length; j++) {
            if (arr[i] == exp[j]) {
                no = true;
                break;
            }
        }
        if (!no) {
            r.push(arr[i]);
        }
    }
    return r;
}


rules.twoAs2= function(arr) {
    var r = [];
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == 15) {
            r.push(2);
        } else {
            r.push(arr[i]);
        }
    }
    return r;
}

