"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var User = /** @class */ (function () {
    function User() {
    }
    User.prototype.getAllRolesAsStr = function () {
        return this.roles.join(', ');
    };
    return User;
}());
exports.User = User;
//# sourceMappingURL=user.js.map