const validatePassword = (password) => {
  const errors = [];

  if (password.length < 6) {
    errors.push({
      code: "PasswordTooShort",
      description: "Passwords must be at least 6 characters.",
    });
  }

  if (!/[^a-zA-Z0-9]/.test(password)) {
    errors.push({
      code: "PasswordRequiresNonAlphanumeric",
      description:
        "Passwords must have at least one non alphanumeric character.",
    });
  }

  if (!/[a-z]/.test(password)) {
    errors.push({
      code: "PasswordRequiresLower",
      description: "Passwords must have at least one lowercase ('a'-'z').",
    });
  }

  if (!/[A-Z]/.test(password)) {
    errors.push({
      code: "PasswordRequiresUpper",
      description: "Passwords must have at least one uppercase ('A'-'Z').",
    });
  }

  return errors;
};

export { validatePassword };
