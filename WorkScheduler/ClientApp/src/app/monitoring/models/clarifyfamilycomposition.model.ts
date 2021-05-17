export enum ClarifyFamilyСomposition{
    marriageRegistered,
    marriageNotRegistered,
    devorced,
    byDeath,
    guardians,
    noGuardians
}

export const clarifyСompositions: any[] = [
  { id: 0, name: 'Брак зарегистрирован' },
  { id: 1, name: 'Брак не зарегистрирован' },
  { id: 2, name: 'В разводе' },
  { id: 3, name: 'В результате смерти одного из супругов' },
  { id: 4, name: 'Имеющие официальное опекунство' },
  { id: 5, name: 'Не имеющие официальное опекунство' }
];