;Copyright (c) 2003, Rich Hickey
;licensed under the BSD license - see license.txt

(Trace:Listeners.Add (TextWriterTraceListener. Console:Error))

(__set list (fn (&rest args) args))	;hmmm, presumes implementation detail

(__set def (macro (var &rest body)
	(Trace:WriteLine (String:Format "defined: {0}" 
						(if (cons? var) (first var) var)))
	(if (cons? var)
		(list '__set (first var) (cons 'fn (cons (rest var) body)))
		(cons '__set (cons var body)))))

;todo - capture test as 'it for use in result?
(def cond (macro (&rest clauses)
	(if (nil? clauses)
		nil
		(if (eql? (first clauses) :else)
			(second clauses)
			(list 'if (first clauses)
				(second clauses)
				(cons 'cond (rest (rest clauses))))))))
					
(def and (macro (&rest args)
	(cond	(nil? args) true
			(nil? (rest args)) (first args)
			:else (list 'if (first args) (cons 'and (rest args)) nil))))

(def (constant? exp) 
	(if (cons? exp) 
			(eql? (first exp) 'quote) 
			(not (symbol? exp)))) 

(def (__starts-with? lst x)
	(and (cons? lst) (eql? (first lst) x)))
	
;from PAIP
(def backquote (macro (x) (__backquote-expand x)))
	
(def (__backquote-expand x)
	(cond
		(atom? x) (if (constant? x) x (list 'quote x))
		(__starts-with? x 'unquote) (second x)
		(__starts-with? x 'backquote) 
			(__backquote-expand (__backquote-expand (second x)))
		(__starts-with? (first x) 'unquote-splicing)
			(if (nil? (rest x))
				(second (first x))
				(list 'append (second (first x)) (__backquote-expand (rest x))))
		:else (__backquote-combine 
						(__backquote-expand (first x)) 
						(__backquote-expand (rest x)) x)))

(def (__backquote-combine	left right x)
	(cond 
		(and (constant? left) (constant? right))
			(if (and (eql? (eval left) (first x)) (eql? (eval right) (rest x)))
				(list 'quote x)
				(list 'quote (cons (eval left) (eval right))))
		(nil? right) (list 'list left)
		(__starts-with? right 'list) (cons 'list (cons left (rest right)))
		:else (list 'cons left right)))

(def def-macro (macro (spec &rest body)
	`(def ~(first spec) (macro ~(rest spec) ~@body))))

(def-macro (nand &rest args)
	`(not (and ~@args)))
	
(def-macro (xor x y)
	`(if ~x (not ~y) ~y))

(def (odd? x)
	(not (even? x)))

(def-macro (error msg)
	`(throw (Exception. ~msg)))

(def (__pairize lst)
     (cond 
		(nil? lst)			nil
		(odd? (len lst))	(error "Expecting even number of arguments")
		:else				(cons (list (first lst) (second lst))
								(__pairize (nth-rest 2 lst)))))

(def-macro (let params &rest body)
	`(__let ~(__pairize params) ~@body))

#|			
(def-macro (__let bindings &rest body)
	`((fn ~(map->list (fn (x)
					(if (atom? (first x))
						(first x)
						(first (first x))))
				bindings)
			~@body)
		~@(map->list (fn (x)
					(if (atom? (first x))
						(second x)
						(list 'fn (rest (first x)) (second x))))
				bindings)))
|#

(def-macro (__let bindings &rest body)
	`((fn ~(map->list first	bindings)
			~@body)
		~@(map->list second	bindings)))
						
(def-macro (lets params &rest body)
	`(__lets ~(__pairize params) ~@body))
			
(def-macro (__lets bindings &rest body) 
	(if (nil? bindings) 
		`((fn () ~@body)) 
      `(let (~@(first bindings)) (__lets ~(rest bindings) ~@body))))

(def-macro (letfn params &rest body)
	`(__letr ~(__pairize params) ~body))

(def-macro (__letr bindings &rest body)
	`(__let ~(map->list (fn (x) (list (first (first x)) 'nil)) bindings)
		~@(concat! (map->list (fn (x) 
						(list '__set (first (first x)) 
								(list 'fn (rest (first x)) (second x))))
					bindings))
				~@body))

;set as a macro
(def (def-setter placefn setfn)
	(placefn.Setter setfn))

(def (setter placefn)
	(if (not (symbol? placefn))
		placefn
		(let (setfn placefn.Setter)
			(if setfn
				setfn
				placefn))))
			
#| gens nested blocks			
(def-macro (__set1 place value &rest args)
              `(block
                ~(if (not (cons? place))
                     `(__set ~place ~value)
                   (let (setfn (setter (first place)))
                     `(~setfn ~@(rest place) ~value)))
                ~@(if args `((__set1 ~@args)) nil)))

(def-macro (set &rest args)
    (if (nil? args)
        nil
      `(__set1 ~@args)))
|#

(def-macro (when arg &rest body)
   `(if ~arg (block ~@body)))

;better, suggested by MH

(def-macro (__set1 place value)
	(if (not (cons? place))
		`(__set ~place ~value)
		(let (setfn (setter (first place)))
			`(~setfn ~@(rest place) ~value))))

(def (__gen-pairwise-calls cmd lst)
	(when lst
		(cons (list cmd (first lst) (second lst))
			(__gen-pairwise-calls cmd (nth-rest 2 lst)))))

(def-macro (set &rest args)
	(when args
		`(block ~@(__gen-pairwise-calls '__set1 args))))

;similar to CL mapcan
(def (mapcat! f &rest lists)
	(apply concat! (apply map->list f lists)))
	
(def (member obj lst &key (test eql?))
	(cond
		(nil? lst) nil
		(test obj (first lst)) lst
		:else (member obj (rest lst) :test test)))

(def (member-if pred lst)
	(cond
		(nil? lst) nil
		(pred (first lst)) lst
		:else (member-if pred (rest lst))))
	
(def-macro (case arg &rest clauses)
	(let (g (gensym))
		`(let (~g ~arg)
			(cond ~@(mapcat! (fn (cl)
							(let (key (first cl))
								`(~(cond 
										(eql? key :else) :else
										(cons? key) `(member ~g '~key)
										:else `(eql? ~g '~key))
									~(second cl))))
							(__pairize clauses))))))

(def (__destructure params args bindings)
	(cond 
		(nil? params) bindings
		(atom? params) (cons `(~params ~args) bindings)
		(cons? params)
			(case (first params)
				(&rest)
					 (cons `(~(second params) ~args) bindings)
				:else
					(__destructure (first params) `(first ~args)
						(__destructure (rest params) `(rest ~args) bindings)))))

(def-macro (destructuring-bind params args &rest body)
	(let (gargs (gensym))
	`(let (~gargs ~args)
		(__let ~(__destructure params gargs nil) ~@body))))

;now redefine def-macro with destructuring
(def (_make_macro params body)
	(let (gargs (gensym))
		`(macro (&rest ~gargs)
			(destructuring-bind ~params ~gargs ~@body)))) 
		
(def (to-bool x)
	(if x true false))
	
(def (tree? lst)
	(to-bool (member-if cons? lst)))
	
(def def-macro (macro (spec &rest body)
	(if (member-if (fn (x) (member x '(&opt &key)))	spec)
		`(def ~(first spec) (macro ~(rest spec) ~@body))
		`(def ~(first spec) ~(_make_macro (rest spec) body)))))

(def-macro (when-not arg &rest body)
   `(if (not ~arg)
       (block ~@body)))

(def-macro (until test &rest body) 
	`(while (not ~test) 
		~@body))

(def-macro (for inits test update &rest body)
	`(lets ~inits
		(while ~test
			(block
				~@body
				~update))))

(def-macro (next! lst)
	`(set ~lst (rest ~lst)))
	
(def-macro (with-gensyms syms &rest body)
    `(__let ~(map->list (fn (s)
                     `(~s (gensym)))
             syms)
       ~@body))
	
(def-macro (dolist var lst &rest body)
	(let (g (gensym))
		`(for (~g ~lst) ~g (next! ~g)
			(let (~var (first ~g)) 
				~@body))))
				
(def-macro (dotails var lst &rest body)
	(let (g (gensym))
		`(for (~g ~lst) ~g (next! ~g)
			(let (~var ~g) 
				~@body))))


;(def-macro (dotimes var n &rest body)
;	(let (gn (gensym))
;		`(for (~gn ~n ~var 0) (< ~var ~gn) ((++ ~var))
;			~@body)))

(def (keyword str)
	(intern (+ ":" str)))
	
(def (__params-to-args params &opt (mode :base))
	(cond 
		(nil? params) nil
		(eqv? (first params) '&opt) (__params-to-args (rest params) :opt)
		(eqv? (first params) '&key) (__params-to-args (rest params) :key)
		(eqv? (first params) '&rest) nil
		:else (case mode
				:base (cons (first params) (__params-to-args (rest params) :base))
				:opt  (cons (if (cons? (first params)) 
								(first (first params))
								(first params))
						(__params-to-args (rest params) :opt))
				:key (cons (if (cons? (first params)) 
								(first (first params))
								(first params))
						(__params-to-args (rest params) :key)))))

(def (__rest-param params)
	(cond 
		(nil? params) nil
		(eqv? (first params) '&rest) (second params)
		:else (__rest-param (rest params))))

(def-macro (def-method (name (p1 dispatch-type-or-value) &rest params) &rest body)
	(when-not name.isDefined
		(name.setGlobalValue (GenericFunction.)))
	`(.AddMethod ~name ~dispatch-type-or-value 
			(fn ~(cons p1 params) 
				(let (call-base-method 
						(fn () 
							(apply (.FindBaseMethod ~name ~dispatch-type-or-value) 
								~p1 ~@(__params-to-args params) ~(__rest-param params))))	
						;((.FindBaseMethod ~name ~dispatch-type-or-value) ~p1 ~@params))	
					~@body))))

(def-macro (def-binop (name (p1 dispatch1) (p2 dispatch2)) &rest body)
	(when-not name.isDefined
		(name.setGlobalValue (BinOp.)))
	`(.AddMethod ~name ~dispatch1 ~dispatch2 
			(fn ~(list p1 p2) 
					~@body)))

;handled in primitives so available for boot
;(def-method (str (obj Object.)) 
;	obj.ToString)

;(def-method (str (obj nil)) 
;	"nil")

(def-method (str (obj true)) 
	"true")

(def-method (str (obj false)) 
	"false")

(def-method (str (obj String.)) 
	(String:Concat "\"" obj "\""))

	
(def *pr-writer Console:Out)
(def *pr-sep " ")

(def (pr &rest x)
	(while (and x (rest x))
		(*pr-writer.Write (str (first x)))
		(*pr-writer.Write *pr-sep)
		(next! x))
	(when x
		(*pr-writer.Write (str (first x)))))
	
(def (prn &rest x)
	(apply pr x)
	(*pr-writer.Write "\n"))

(def (prs &rest x)
	(while (and x (rest x))
		(*pr-writer.Write  (first x))
		(*pr-writer.Write *pr-sep)
		(next! x))
	(when x
		(*pr-writer.Write (first x))))
	
(def (prns &rest x)
	(apply prs x)
	(*pr-writer.Write "\n"))

(def-macro (+= x n)
	`(set ~x (add ~x ~n)))

(def one 1)

(def-macro (++ x)
	`(+= ~x one))

(def-macro (__accum op args)
	(with-gensyms (x result)
		`(let (~result (first ~args))
			(dolist ~x (rest ~args)
				(~op ~result ~x))
			~result)))
	
(def (+ &rest args)
	(__accum += args))
	
(def-macro (-= x n)
	`(set ~x (subtract ~x ~n)))

(def-macro (-- x)
	`(-= ~x 1))
	
(def (- &rest args)
	(__accum -= args))

(def-macro (*= x n)
	`(set ~x (multiply ~x ~n)))

(def (* &rest args)
	(__accum *= args))

(def-macro (/= x n)
	`(set ~x (divide ~x ~n)))

(def (/ &rest args)
	(__accum /= args))


	
(def (now) DateTime:Now)

(def-macro (dotimes var n &rest body)
	(with-gensyms (gn)
		`(for (~var 0 ~gn ~n) (< ~var ~gn) (++ ~var)
			~@body)))

(def (zero? x)
	(== x 0))
(def (positive? x)
	(> x 0))
(def (negative? x)
	(< x 0))
	
(def (__min x y)
	(if (< x y) x y))

(def (min &rest args)
	(let (result (first args))
		(for (args (rest args)) args (next! args)
			(set result (__min result (first args))))
		result))

(def (__max x y)
	(if (> x  y) x y))

(def (max &rest args)
	(let (result (first args))
		(while (rest args)
			(set args (rest args))
			(set result (__max result (first args))))
		result))


(def-macro (push! obj place)
    `(set ~place (cons ~obj ~place)))

(def-macro (pop! place)
    `(set ~place (rest ~place)))

(def-macro (rotate-set &rest args)
    `(parallel-set ~@(mapcat! list
               args
               (append (rest args) 
                       (list (first args))))))

;todo reverse and reverse! need to be gfuncs
(def (reverse! lst)
     (let (prev nil)
       (while (cons? lst)
         (parallel-set (rest lst) prev
               prev      lst
               lst       (rest lst)))
		prev))

(def (butlast lst &opt (n 1))
     (reverse! (nth-rest n (reverse lst))))

(def (last lst &opt (n 1))
	(let (l (len lst))
		(if	(<= l n) 
			lst
			(nth-rest (- l n) lst))))
		
