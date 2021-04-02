import { Greeter } from './greeter';

let greeter: Greeter = new Greeter('Rob');
let greeting: string = greeter.greet();

console.log(greeting);