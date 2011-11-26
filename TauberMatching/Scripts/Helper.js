/**
 * @fileOverview Js helper function implementations which are mostly found from googling around
 * @author Melih Gunal 11/26/2011
 */

/**
 * @function String prototype function to implement C# String.Format like function. usage: "{0} is dead, but {1} is alive! {0} {2}".format("ASP", "ASP.NET")
 * @see <a href="http://stackoverflow.com/questions/610406/javascript-equivalent-to-printf-string-format/4673436#4673436">Author's comments at Stackoverflow</a>
 */
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
      ? args[number]
      : match
    ;
    });
};

/**
 * @function Strips html from a given string
 * @see <a href="http://css-tricks.com/snippets/javascript/strip-html-tags-in-javascript/">Strip HTML Tags at css-tricks.com</a>
 */
function stripHtmlUsingRegex(inputString) {
    inputString.replace(/(<([^>]+)>)/ig, "");
    // or try .replace(/<(?:.|\n)*?>/gm, '');
    // or try 
}
/**
 * @function Strip html from input string using browser.
 * @see <a href="http://stackoverflow.com/questions/822452/strip-html-from-text-javascript">Shog9 entry at StackOverflow</a>
 */
function stripUsingBrowser(html) {
    var tmp = document.createElement("DIV");
    tmp.innerHTML = html;
    return tmp.textContent || tmp.innerText;
}
