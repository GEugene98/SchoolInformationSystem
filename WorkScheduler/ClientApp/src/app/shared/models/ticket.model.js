"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var dictionary_model_1 = require("./dictionary.model");
var Ticket = /** @class */ (function (_super) {
    __extends(Ticket, _super);
    function Ticket() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return Ticket;
}(dictionary_model_1.Dictionary));
exports.Ticket = Ticket;
//# sourceMappingURL=ticket.model.js.map