import React, { ButtonHTMLAttributes, forwardRef } from "react";
import classNames from "classnames";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: "default" | "outline";
}

export const Button = forwardRef<HTMLButtonElement, ButtonProps>(
  ({ variant = "default", className, ...props }, ref) => {
    const buttonClass = classNames(
      "px-4 py-2 rounded text-white font-medium transition",
      {
        "bg-blue-500 hover:bg-blue-600": variant === "default",
        "bg-transparent border border-blue-500 text-blue-500 hover:bg-blue-100":
          variant === "outline",
      },
      className
    );

    return (
      <button ref={ref} className={buttonClass} {...props}>
        {props.children}
      </button>
    );
  }
);

Button.displayName = "Button";
